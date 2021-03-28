using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text scoreText = null;
    private int oldScore = 0;
    private GManager gm = null;
    // Start is called before the first frame update
    void Start()
    {
        gm = GManager.GetInstance();
        scoreText = GetComponent<Text>();
        if(gm != null)
        {
            SetScoreText(gm.score);
        }
        else
        {
            Debug.Log("ゲームマネージャがありません");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(oldScore != gm.score)
        {
            SetScoreText(gm.score);
            oldScore = gm.score;
        }
    }

    private void SetScoreText(int score)
    {
        scoreText.text = "Score:" + String.Format("{0:D4}", score);
    }
}
