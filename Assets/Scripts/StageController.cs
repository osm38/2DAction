using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")] public GameObject playerObj;
    [Header("コンティニュー位置")] public GameObject[] continuePoint;
    [Header("ゲームオーバー")] public GameObject gameOverObj;
    [Header("フェード")] public FadeImage fade;
    [Header("ゲームオーバSE")] public AudioClip gameOverSE;
    [Header("リトライSE")] public AudioClip retrySE;
    [Header("ステージクリアSE")] public AudioClip stageClearSE;
    [Header("ステージクリア")] public GameObject stageClearObj;
    [Header("ステージクリア判定")] public PlayerTriggerCheck stageClearTrigger;

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
                Debug.Log("プレイヤー以外のオブジェクトがアタッチされています");
            }
        }
        else
        {
            Debug.Log("設定が足りていません");
        }
    }

    private void Update()
    {
        // ゲームオーバー時の処理
        if(gm.isGameOver && !doGameOver)
        {
            gameOverObj.SetActive(true);
            gm.PlaySE(gameOverSE);
            doGameOver = true;
        }
        // プレイヤーがやられたときの処理
        else if(p != null && p.IsContinueWaiting() && !doGameOver)
        {
            if(continuePoint.Length > gm.continueNum)
            {
                playerObj.transform.position = continuePoint[gm.continueNum].transform.position;
                p.ContinuePlayer();
            }
            else
            {
                Debug.Log("コンティニューポイントの設定が足りていません");
            }
        }
        // ステージクリア時の処理
        else if(stageClearTrigger != null && stageClearTrigger.isOn && !doGameOver && !doClear)
        {
            StageClear();
            doClear = true;
        }

        // ステージを切り替える
        if(fade != null && startFade && !doSceneChange)
        {
            if (fade.IsFadeOutComplete())
            {
                // ゲームリトライ
                if (retryGame)
                {
                    gm.RetryGame();
                }
                // 次のステージ
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
    /// 最初から始める
    /// </summary>
    public void Retry()
    {
        gm.PlaySE(retrySE);
        ChangeScene(1);
        retryGame = true;
    }

    /// <summary>
    /// ステージを切り替えます
    /// </summary>
    /// <param name=="stageNo">ステージ番号</param>
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
    /// ステージをクリアした
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
