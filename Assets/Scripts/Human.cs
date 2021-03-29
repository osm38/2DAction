using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    #region // �C���X�y�N�^�[�Őݒ肷��
    [Header("�ړ����x")]public float speed;
    [Header("�d��")]public float gravity; 
    [Header("�W�����v���x")]public float jumpSpeed; 
    [Header("�W�����v���鍂��")]public float jumpHeight; 
    [Header("�W�����v��������")]public float jumpLimitTime; 
    [Header("�ڒn����")]public GroundCheck ground; 
    [Header("������")]public GroundCheck head; 
    [Header("�_�b�V���̑����\��")]public AnimationCurve dashCurve;
    [Header("�W�����v�̑����\��")]public AnimationCurve jumpCurve;
    [Header("���݂�����̍����̊���")] public float stepOnRate;
    [Header("�W�����vSE")] public AudioClip jumpSE;
    [Header("�_�E��SE")] public AudioClip downSE;
    [Header("�R���e�B�j���[SE")] public AudioClip continueSE;
    #endregion

    #region // �v���C�x�[�g�ϐ�
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
        // �R���|�[�l���g�̃C���X�^���X�擾
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
            // �ڒn������擾
            isGround = ground.IsGround();
            isHead = head.IsGround();

            // �e����W���̑��x�����߂�
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            // �A�j���[�V������K�p
            SetAnimation();

            // �ړ����x��ݒ�
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
            // ���� ���Ă��鎞�ɖ߂�
            if(blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            // ���� �����Ă��鎞
            else if(blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            // ���� ���Ă���Ƃ�
            else
            {
                sr.enabled = true;
            }

            // 1�b�o�����疾�ŏI���
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
    /// Y�����ŕK�v�Ȍv�Z�����A���x��Ԃ��B
    /// </summary>
    /// <returns>Y���̑���</returns>
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical");
        float ySpeed = -gravity;

        // �����𓥂񂾂Ƃ��̃W�����v
        if (isOtherJump)
        {
            // ���݂̍�������ׂ鍂����艺��
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //�W�����v���Ԃ������Ȃ肷���Ă��Ȃ���
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
                jumpPos = transform.position.y; // �W�����v�����ʒu���L�^
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
            // ������L�[�������Ă��邩
            bool pushUpKey = verticalKey > 0;
            // ���݂̍�������ׂ鍂����艺��
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            // �W�����v���Ԃ������Ȃ肷���Ă��Ȃ���
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
    /// X�����ŕK�v�Ȍv�Z�����A���x��Ԃ��B
    /// </summary>
    /// <returns>X���̑���</returns>
    private float GetXSpeed()
    {
        float xSpeed = 0.0f;
        float horizontalKey = Input.GetAxis("Horizontal");
        // ���E�ړ�
        if (horizontalKey != 0)
        {
            // �E����
            if (horizontalKey > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                xSpeed = speed;
            }
            // ������
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

        // �O��̓��͂���_�b�V���̔���𔻒f���đ��x��ς���
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }
        beforeKey = horizontalKey;
        // �A�j���[�V�����J�[�u�𑬓x�ɓK�p
        xSpeed *= dashCurve.Evaluate(dashTime);

        return xSpeed;

    }

    private void SetAnimation()
    {
        animator.SetBool("run", isRun || isOtherJump);
        animator.SetBool("jump", isJump);
        animator.SetBool("ground", isGround);
    }

    #region // �ڐG����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool jumpStep = (collision.collider.tag == jumpStepTag);
        if (enemy || moveFloor || fallFloor || jumpStep)
        {
            // ���݂�����ɂȂ鍂��
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));

            // ���݂�����̃��[���h���W
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach(ContactPoint2D p in collision.contacts){
                // �G�̍������A�v���C���[�̍����̂ق����傫���ꍇ
                if(p.point.y < judgePos)
                {
                    if(enemy || fallFloor || jumpStep)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();

                        if (o != null)
                        {
                            if (enemy || jumpStep)
                            {
                                otherJumpHeight = o.boundHeight; // ����Â������̂��璵�˂鍂�����擾����
                                otherJumpSpeed = o.jumpSpeed;
                                o.playerStepOn = true; // ����Â������̂ɑ΂��ē���Â������Ƃ�ʒm����
                                jumpPos = transform.position.y; // �W�����v�����ʒu���L�^����
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
                            Debug.Log("ObjectCollision���t���ĂȂ���I");
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
            // ���������痣�ꂽ
            moveObj = null;
        }
    }

    /// <summary>
    /// �R���e�B�j���[�ҋ@��Ԃ�
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

    // �_�E���A�j���[�V�������������Ă��邩�ǂ���
    private bool IsDownAnimEnd()
    {
        if(isDown && animator != null)
        {
            // ���ݍĐ����̃A�j���[�V���������擾
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            // �Đ����̃A�j���[�V�������_�E���A�j���[�V�����ł��邩
            if (currentState.IsName("human_down"))
            {
                // �A�j���[�V�������I�����Ă��邩
                // �A�j���[�V�����S�̂�1�Ƃ����ꍇ����ȏォ
                if (currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// �R���e�B�j���[����
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

    // ���ꂽ�Ƃ��̏���
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
