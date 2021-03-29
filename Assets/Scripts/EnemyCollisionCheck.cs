using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    /// <summary>
    /// ”»’è“à‚É“G‚©•Ç‚ª‚ ‚é
    /// </summary>
    [HideInInspector] public bool isOn = false;

    #region // ÚG”»’è
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.containsEnemyWrapTag())
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.containsEnemyWrapTag())
        {
            isOn = false;
        }
    }
    #endregion
}
