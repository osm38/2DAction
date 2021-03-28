using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    private ObjectCollision oc;
    private Animator anim;
    void Start()
    {
        oc = GetComponent<ObjectCollision>();
        anim = GetComponent<Animator>();
        if(oc == null || anim == null)
        {
            Debug.Log("ƒWƒƒƒ“ƒv‘ä‚Ìİ’è‚ª‘«‚è‚Ä‚¢‚Ü‚¹‚ñ");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (oc.playerStepOn)
        {
            anim.SetTrigger("on");
            oc.playerStepOn = false;
        }
    }
}
