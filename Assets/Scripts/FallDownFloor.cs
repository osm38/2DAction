using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownFloor : MonoBehaviour
{
    [Header("�X�v���C�g������I�u�W�F�N�g")] public GameObject spriteObject;
    [Header("�U����")] public float vibrationWidth = 0.05f;
    [Header("�U�����x")] public float vibrationSpeed = 30.0f;
    [Header("������܂ł̎���")] public float fallTime = 1.0f;
    [Header("�����Ă������x")] public float fallSpeed = 10.0f;
    [Header("�����Ă���߂��Ă��鎞��")] public float returnTime = 5.0f;

    private bool isOn;
    private bool isFall;
    private bool isReturn;
    private Vector3 spriteDefaultPos;
    private Vector3 floorDefaultPos;
    private Vector2 fallVelocity;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private ObjectCollision oc;
    private SpriteRenderer sr;
    private float timer = 0.0f;
    private float fallingTimer = 0.0f;
    private float returnTimer = 0.0f;
    private float blinkTimer = 0.0f;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        oc = GetComponent<ObjectCollision>();
        if(spriteObject != null && oc != null && col != null && rb != null)
        {
            spriteDefaultPos = spriteObject.transform.position;
            fallVelocity = new Vector2(0, -fallSpeed);
            floorDefaultPos = gameObject.transform.position;
            sr = spriteObject.GetComponent<SpriteRenderer>();
            if(sr == null)
            {
                Debug.Log("fallDownFloor �C���X�y�N�^�[�ɐݒ肵�Y�ꂪ����܂�");
                Destroy(this);
            }
        }
        else
        {
            Debug.Log("fallDownFloor �C���X�y�N�^�[�ɐݒ肵�Y�ꂪ����܂�");
            Destroy(this);
        }
    }

    void Update()
    {
        // �v���C���[��1��ł��������t���O���I����
        if (oc.playerStepOn)
        {
            isOn = true;
            oc.playerStepOn = false;
        }

        // �v���C���[������Ă��痎����܂ł̊�
        if(isOn && !isFall)
        {
            // �k������
            spriteObject.transform.position = spriteDefaultPos + new Vector3(Mathf.Sin(timer * vibrationSpeed) * vibrationWidth, 0, 0);

            // ��莞�Ԍo�����痎����
            if (timer > fallTime)
            {
                isFall = true;
            }

            timer += Time.deltaTime;
        }

        // ��莞�Ԍo�Ɩ��ł��Ė߂��Ă���
        if (isReturn)
        {
            // ���� ���Ă��鎞�ɖ߂�
            if(blinkTimer > 0.2f)
            {
                sr.enabled = true;
                blinkTimer = 0.0f;
            }
            // ���� �����Ă��鎞
            else if(blinkTimer > 0.1f)
            {
                sr.enabled = false;
            }
            // ���� ���Ă��鎞
            else
            {
                sr.enabled = true;
            }

            // 1�b�o������I���
            if(returnTimer > 1.0f)
            {
                isReturn = false;
                blinkTimer = 0f;
                returnTimer = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTimer += Time.deltaTime;
                returnTimer += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // ������
        if (isFall)
        {
            rb.velocity = fallVelocity;

            // ��莞�Ԍo�ƌ��̈ʒu�ɖ߂�
            if(fallingTimer > returnTime)
            {
                isReturn = true;
                transform.position = floorDefaultPos;
                rb.velocity = Vector2.zero;
                isFall = false;
                timer = 0.0f;
                fallingTimer = 0.0f;
            }
            else
            {
                fallingTimer += Time.deltaTime;
                isOn = false;
            }
        }
    }
}