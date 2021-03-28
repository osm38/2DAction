using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZako1 : MonoBehaviour
{
    #region // インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("死亡SE")] public AudioClip deadSE;
    #endregion

    #region // プライベート変数
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
                // 接触している場合は逆を向く
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
