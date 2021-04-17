using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("�X�N���[���X�s�[�h")] public float speed = 0.1f;

    // Update is called once per frame
    void Update()
    {
        // ���Ԃɂ����X�̒l��0����1�ɕω����Ă����B1�ɂȂ�����0�ɖ߂�J��Ԃ��B
        float x = Mathf.Repeat(Time.time * speed, 1);

        // X�̒l������Ă����I�t�Z�b�g���쐬�B
        Vector2 offset = new Vector2(x, 0);

        // �}�e���A���ɃI�t�Z�b�g��ݒ肷��B
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

    }
}
