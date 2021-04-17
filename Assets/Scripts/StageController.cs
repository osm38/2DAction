using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [Header("�v���C���[�Q�[���I�u�W�F�N�g")] public GameObject playerObj;
    [Header("�R���e�B�j���[�ʒu")] public GameObject[] continuePoint;
    [Header("�Q�[���I�[�o�[")] public GameObject gameOverObj;
    [Header("�t�F�[�h")] public FadeImage fade;
    [Header("�Q�[���I�[�oSE")] public AudioClip gameOverSE;
    [Header("���g���CSE")] public AudioClip retrySE;
    [Header("�X�e�[�W�N���ASE")] public AudioClip stageClearSE;
    [Header("�X�e�[�W�N���A")] public GameObject stageClearObj;
    [Header("�X�e�[�W�N���A����")] public PlayerTriggerCheck stageClearTrigger;

    private Human p;
    private GManager gm;
    private int nextStageNo;
    private bool startFade = false;
    private bool doGameOver = false;
    private bool retryGame = false;
    private bool doSceneChange = false;
    private bool doClear = false;

    void Start()
    {
        if(playerObj != null && continuePoint != null && continuePoint.Length > 0)
        {
            gameOverObj.SetActive(false);
            stageClearObj.SetActive(false);
            playerObj.transform.position = continuePoint[0].transform.position;
            gm = GManager.GetInstance();
            p = playerObj.GetComponent<Human>();
            if(p == null)
            {
                Debug.Log("�v���C���[�ȊO�̃I�u�W�F�N�g���A�^�b�`����Ă��܂�");
            }
        }
        else
        {
            Debug.Log("�ݒ肪����Ă��܂���");
        }
    }

    private void Update()
    {
        // �Q�[���I�[�o�[���̏���
        if(gm.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            gm.PlaySE(gameOverSE);
            doGameOver = true;
        }
        // �v���C���[�����ꂽ�Ƃ��̏���
        else if(p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if(continuePoint.Length > gm.continueNum)
            {
                playerObj.transform.position = continuePoint[gm.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("�R���e�B�j���[�|�C���g�̐ݒ肪����Ă��܂���");
            }
        }
        // �X�e�[�W�N���A���̏���
        else if(stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }

        // �X�e�[�W��؂�ւ���
        if(fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                // �Q�[�����g���C
                if (retryGame)
                {
                    gm.RetryGame();
                }
                // ���̃X�e�[�W
                else
                {
                    gm.stageNo = nextStageNo;
                }
                gm.isStageClear = false;
                if(nextStageNo >= 2)
                {
                    SceneManager.LoadScene("title");
                }
                else
                {
                    SceneManager.LoadScene("stage" + nextStageNo);
                }
                doSceneChange = true;
            }
        }
    }

    /// <summary>
    /// �ŏ�����n�߂�
    /// </summary>
    public void Retry()
    {
        gm.PlaySE(retrySE);
        ChangeScene(1);
        retryGame = true;
    }

    /// <summary>
    /// �X�e�[�W��؂�ւ��܂�
    /// </summary>
    /// <param name=="stageNo">�X�e�[�W�ԍ�</param>
    public void ChangeScene(int stageNo)
    {
        if(fade != null)
        {
            nextStageNo = stageNo;
            fade.StartFadeOut();
            startFade = true;
        }
    }

    /// <summary>
    /// �X�e�[�W���N���A����
    /// </summary>
    public void StageClear()
    {
        gm.isStageClear = true;
        stageClearObj.SetActive(true);
        gm.PlaySE(stageClearSE);
    }

    public void EndGame()
    {
        gm.EndGame();
    }
}
