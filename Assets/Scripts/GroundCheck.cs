using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("エフェクトが付いた床を判定するか")] public bool checkPlatformGround = true;
    [Header("落ちそうなのを判定するか")] public bool checkFall = true;

    private string groundTag = Tag.Ground.ToString();
    private string platformTag = Tag.GroundPlatform.ToString();
    private string moveFloorTag = Tag.MoveFloor.ToString();
    private string fallFloorTag = Tag.FallFloor.ToString();
    private bool isGround = false;
    private bool isAboutToFall = false;
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
     * 接地判定を返すメソッド
     * 物理判定の更新ごとに呼ぶ必要がある。
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

    ///<summary>
    ///落ちそうかどうかを判定するメソッド
    /// </summary>
    public bool IsAboutToFall()
    {
        if (checkFall && isGroundExit)
        {
            isAboutToFall = true;
            isGroundExit = false;
        }
        else
        {
            isAboutToFall = false;
        }

        return isAboutToFall;
    }

}
