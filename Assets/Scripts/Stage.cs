using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    private GManager gm = null;
    private Text stageText = null;
    private int oldStageNo = 0;
    // Start is called before the first frame update
    void Start()
    {
        gm = GManager.GetInstance();
        stageText = GetComponent<Text>();
        if(gm != null)
        {
            SetStageText(gm.stageNo);
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
        if(gm.stageNo != oldStageNo)
        {
            SetStageText(gm.stageNo);
        }
    }

    private void SetStageText(int stageNo)
    {
        this.stageText.text = "Stage " + stageNo;
    }
}
