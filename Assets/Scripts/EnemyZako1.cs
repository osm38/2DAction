using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZako1 : MonoBehaviour
{
    #region // �C���X�y�N�^�[�Őݒ肷��
    [Header("���Z�X�R�A")] public int myScore;
    [Header("�ړ����x")] public float speed;
    [Header("�d��")] public float gravity;
    [Header("��ʊO�ł��s������")] public bool nonVisibleAct;
    [Header("�ڐG����")] public EnemyCollisionCheck checkCollision;
    [Header("���SSE")] public AudioClip deadSE;
    #endregion

    #region // �v���C�x�[�g�ϐ�
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator animator = null;
    private ObjectCollision oc = null;
    private BoxCollider2D bcol = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    private GManager gm = null;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        bcol = GetComponent<BoxCollider2D>();
        gm = GManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (!oc.playerStepOn)
        {
            if (sr.isVisible || nonVisibleAct)
            {
                // �ڐG���Ă���ꍇ�͋t������
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.velocity = new Vector2(xVector * speed, -gravity);
            }
            else
            {
                rb.Sleep();
            }
        }
        else
        {
            if (!isDead)
            {
                animator.Play("enemy_down");
                rb.velocity = new Vector2(0, -gravity);
                isDead = true;
                bcol.enabled = false;
                gm.PlaySE(deadSE);
                gm.AddScore(myScore);
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
    }
}
