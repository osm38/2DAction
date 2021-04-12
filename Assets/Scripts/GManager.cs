using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{

    private static GManager instance = null;
    [Header("スコア")] public int score;
    [Header("ステージ番号")] public int stageNo;
    [Header("復帰位置")] public int continueNum;
    [Header("残機")] public int life;
    [Header("デフォルト残機")] public int defaultLife;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isStageClear = false;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // シーン切替時に破棄されないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static GManager GetInstance()
    {
        return instance;
    }

    public void AddScore(int score)
    {
        this.score += score;

    }

    public void SubtractScore(int score)
    {
        this.score -= score;
    }

    public void ResetScore(int score)
    {
        this.score = 0;
    }

    public void AddLife(int life)
    {
        if(this.life < 99)
        {
            this.life += life;
        }
    }
    public void SubtractLife(int life)
    {
        if(this.life > 0)
        {
            this.life -= life;
        }
        else
        {
            isGameOver = true;
        }
    }
    public void ResetLife()
    {
        this.life = defaultLife;
    }

    public void RetryGame()
    {
        isGameOver = false;
        ResetLife();
        score = 0;
        stageNo = 1;
        continueNum = 0;
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
    }

    public void EndGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
