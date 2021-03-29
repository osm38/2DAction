using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tag { 
    Player,
    Enemy,
    Ground,
    GroundPlatform,
    DeadArea,
    HitArea,
    MoveFloor,
    FallFloor,
    JumpStep,
}

public static class Collider2DExt
{
    public static bool containsEnemyWrapTag(this Collider2D col)
    {
        Tag[] wrapTags = { Tag.Ground, Tag.DeadArea, Tag.HitArea, Tag.JumpStep };

        try
        {
            var nameTag = Enum.Parse(typeof(Tag), col.tag);
            return 0 <= Array.IndexOf(wrapTags,nameTag);
        }
        catch(ArgumentException)
        {
            return false;
        }
    }
}