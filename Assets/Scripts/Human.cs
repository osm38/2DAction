using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    #region // インスペクターで設定する
    [Header("移動速度")]public float speed;
    [Header("重力")]public float gravity; 
    [Header("ジャンプ速度")]public float jumpSpeed; 
    [Header("ジャンプする高さ")]public float jumpHeight; 
    [Header("ジャンプ制限時間")]public float jumpLimitTime; 
    [Header("接地判定")]public GroundCheck ground; 
    [Header("頭判定")]public GroundCheck head; 
    [Header("ダッシュの速さ表現")]public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")]public AnimationCurve jumpCurve;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("ジャンプSE")] public AudioClip jumpSE;
    [Header("ダウンSE")] public AudioClip downSE;
    [Header("コンティニューSE")] public AudioClip continueSE;
    #endregion

    #region // プライベート変数
    private Animator animator = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;
    private GManager gm = null;
    private SpriteRenderer sr = null;
    private MoveObject moveObj = null;
    private bool isGround = false;
    private bool isHead = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isDown = false;
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    private float jumpPos = 0.0f;
    private float dashTime, jumpTime;
    private float beforeKey;
    private float otherJumpHeight = 0.0f;
    private float otherJumpSpeed = 0.0f;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private string enemyTag = Tag.Enemy.ToString();
    private string deadAreaTag = Tag.DeadArea.ToString();
    private string hitAreaTag = Tag.HitArea.ToString();
    private string moveFloorTag = Tag.MoveFloor.ToString();
    private string fallFloorTag = Tag.FallFloor.ToString();
    private string jumpStepTag = Tag.JumpStep.ToString();
    #endregion

    void Start()
    {
        // コンポーネントのインスタンス取得
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        gm = GManager.GetInstance();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDown && !gm.isGameOver && !gm.isStageClear)
        {
            // 接地判定を取得
            isGround = ground.IsGround();
            isHead = head.IsGround();

            // 各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            // アニメーションを適用
            SetAnimation();

            // 移動速度を設定
            Vector2 addVeclocity = Vector2.zero;
            if(moveObj != null)
            {
                addVeclocity = moveObj.GetVelocity();
            }
            rb.velocity = new Vector2(xSpeed, ySpeed) + addVeclocity;
        }
        else
        {
            if(!isClearMotion && gm.isStageClear)
            {
                animator.Play("human_rejoice");
                isClearMotion = true;
            }
            rb.velocity = new Vector2(0, -gravity);
        }

    }

    private void Update()
    {
        if (isContinue)
        {
            // 明滅 ついている時に戻る
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            // 明滅 消えている時
            else if(blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            // 明滅 ついているとき
            else
            {
                sr.enabled = true;
            }

            // 1秒経ったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Y成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>Y軸の速さ</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        // 何かを踏んだときのジャンプ
        if (isOtherJump)
        {
            // 現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead)
            {
                ySpeed = otherJumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        else if (isGround)
        {
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    gm.PlaySE(jumpSE);
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; // ジャンプした位置を記録
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump)
        {
            // 上方向キーを押しているか
            bool pushUpKey = verticalKey > 0;
            // 現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            // ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }

        return ySpeed;
    }
    /// <summary>
    /// X成分で必要な計算をし、速度を返す。
    /// </summary>
    /// <returns>X軸の速さ</returns>
    private float GetXSpeed()
    {
        float xSpeed = 0.0f;
        float horizontalKey = Input.GetAxis("Horizontal");
        // 左右移動
        if (horizontalKey != 0)
        {
            // 右方向
            if (horizontalKey > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                xSpeed = speed;
            }
            // 左方向
            else if (horizontalKey < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                xSpeed = -speed;
            }
            dashTime += Time.deltaTime;
            isRun = true;
        }
        else
        {
            isRun = false;
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        // 前回の入力からダッシュの判定を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;
        // アニメーションカーブを速度に適用
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;

    }

    private void SetAnimation()
    {
        animator.SetBool("run", isRun || isOtherJump);
        animator.SetBool("jump", isJump);
        animator.SetBool("ground", isGround);
    }

    #region // 接触判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool jumpStep = (collision.collider.tag == jumpStepTag);
        if (enemy || moveFloor || fallFloor || jumpStep)
        {
            // 踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            // 踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach(ContactPoint2D p in collision.contacts){
                // 敵の高さより、プレイヤーの高さのほうが大きい場合
                if(p.point.y < judgePos)
                {
                    if(enemy || fallFloor || jumpStep)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();

                        if (o != null)
                        {
                            if (enemy || jumpStep)
                            {
                                otherJumpHeight = o.boundHeight; // 踏んづけたものから跳ねる高さを取得する
                                otherJumpSpeed = o.jumpSpeed;
                                o.playerStepOn = true; // 踏んづけたものに対して踏んづけたことを通知する
                                jumpPos = transform.position.y; // ジャンプした位置を記録する
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor)
                            {
                                o.playerStepOn = true;
                            }
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionが付いてないよ！");
                        }
                    }else if (moveFloor)
                    {
                        moveObj = collision.gameObject.GetComponent<MoveObject>();
                    }
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
        }
    }
    #endregion

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag)
        {
            // 動く床から離れた
            moveObj = null;
        }
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <returns></returns>
    public bool IsContinueWaiting()
    {
        if (gm.isGameOver)
        {
            return IsDownAnimEnd();
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }

    // ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if(isDown && animator != null)
        {
            // 現在再生中のアニメーション情報を取得
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            // 再生中のアニメーションがダウンアニメーションであるか
            if (currentState.IsName("human_down"))
            {
                // アニメーションが終了しているか
                // アニメーション全体を1とした場合それ以上か
                if (currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    public void ContinuePlayer()
    {
        gm.PlaySE(continueSE);
        isDown = false;
        animator.Play("human_default");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    // やられたときの処理
    private void ReceiveDamage(bool downAnim)
    {
        if (isDown || gm.isStageClear)
        {
            return;
        }
        else
        {
            if (downAnim)
            {
                animator.Play("human_down");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            gm.PlaySE(downSE);
            gm.SubtractLife(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
        }
        else if(collision.tag == hitAreaTag)
        {
            ReceiveDamage(true);
        }
    }
}
