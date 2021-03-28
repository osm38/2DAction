using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [Header("これを踏んだときのプレイヤーが跳ねる高さ")] public float boundHeight;
    [Header("これを踏んだときのプレイヤーが跳ねる速さ")] public float jumpSpeed;
    /// <summary>
    /// このオブジェクトをプレイヤーが踏んだかどうか
    /// </summary>
    [HideInInspector]public bool playerStepOn;
}
