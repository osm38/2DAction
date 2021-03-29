using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("�G�t�F�N�g���t�������𔻒肷�邩")] public bool checkPlatformGround = true;

    private string groundTag = Tag.Ground.ToString();
    private string platformTag = Tag.GroundPlatform.ToString();
    private string moveFloorTag = Tag.MoveFloor.ToString();
    private string fallFloorTag = Tag.FallFloor.ToString();
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGroundEnter = true;
        }
        else if(checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag)){
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == groundTag)
        {
            isGroundStay = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
        else if (checkPlatformGround && (collision.tag == platformTag || collision.tag == moveFloorTag || collision.tag == fallFloorTag))
        {
            isGroundExit = true;
        }
    }

    /**
     * �ڒn�����Ԃ����\�b�h
     * ��������̍X�V���ƂɌĂԕK�v������B
     */
    public bool IsGround()
    {
        if(isGroundEnter || isGroundStay)
        {
            isGround = true;
        } else if (isGroundExit)
        {
            isGround = false;
        }
        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;

        return isGround;
    }

}
