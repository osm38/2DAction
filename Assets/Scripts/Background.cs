using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("スクロールスピード")] public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        // 時間によってXの値が0から1に変化していく。1になったら0に戻り繰り返す。
        float x = Mathf.Repeat(Time.time * speed, 1);

        // Xの値がずれていくオフセットを作成。
        Vector2 offset = new Vector2(x, 0);

        // マテリアルにオフセットを設定する。
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

    }
}
