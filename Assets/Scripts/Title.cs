using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("�t�F�[�h")]public FadeImage fade;
    [Header("�Q�[���X�^�[�gSE")] public AudioClip startSE;

    private bool firstPush = false;
    private bool goNextScene = false;
    private GManager gm = null;

    private void Start()
    {
        gm = GManager.GetInstance();
    }

    // �X�^�[�g�{�^���������ꂽ��Ă΂��
    public void PressStart()
    {
        Debug.Log("Press Start!");
        if (!firstPush)
        {
            gm.PlaySE(startSE);
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    void Update()
    {
        if(!goNextScene && fade.IsFadeOutComplete())
        {
            gm.RetryGame();
            SceneManager.LoadScene("stage1");
            goNextScene = true;
        }
    }

    public void EndGame()
    {
        gm.EndGame();
    }
}
