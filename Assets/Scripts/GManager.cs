using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{

    private static GManager instance = null;
    [Header("�X�R�A")] public int score;
    [Header("�X�e�[�W�ԍ�")] public int stageNo;
    [Header("���A�ʒu")] public int continueNum;
    [Header("�c�@")] public int life;
    [Header("�f�t�H���g�c�@")] public int defaultLife;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isStageClear = false;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // �V�[���ؑ֎��ɔj������Ȃ��悤�ɂ���
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
    /// SE��炷
    /// </summary>
    public void PlaySE(AudioClip clip)
    {
        if(audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��Ă��܂���");
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
