using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("���Z�X�R�A")] public int myScore;
    [Header("�擾SE")] public AudioClip coinSE;

    private GManager gm = null;
    private PlayerTriggerCheck ptc = null;
    // Start is called before the first frame update
    void Start()
    {
        ptc = GetComponent<PlayerTriggerCheck>();
        gm = GManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (ptc.isOn)
        {
            gm.PlaySE(coinSE);
            gm.AddScore(myScore);
            Destroy(this.gameObject);
        }
    }
}
