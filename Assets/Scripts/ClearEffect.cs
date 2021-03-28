using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEffect : MonoBehaviour
{
    [Header("�g��k���̃A�j���[�V�����J�[�u")] public AnimationCurve curve;
    [Header("�X�e�[�W�R���g���[���[")] public StageController ctrl;
    private bool comp = false;
    private float timer;
    private GManager gm;
    void Start()
    {
        transform.localScale = Vector3.zero;
        gm = GManager.GetInstance();
    }

    void Update()
    {
        if (!comp)
        {
            if (timer < 1.0f)
            {
                transform.localScale = Vector3.one * curve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.one;
                ctrl.ChangeScene(gm.stageNo + 1);
                comp = true;
            }
        }
    }
}
