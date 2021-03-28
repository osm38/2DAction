using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private GManager gm = null;
    private int oldLife = 0;
    private Text life;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GManager.GetInstance();
        life = GetComponent<Text>();
        if(gm != null)
        {
            SetLifeText(gm.life);
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
        if(gm.life != oldLife)
        {
            SetLifeText(gm.life);
            oldLife = gm.life;
        }
    }

    private void SetLifeText(int life)
    {
        this.life.text = "× " + life;
    }
}
