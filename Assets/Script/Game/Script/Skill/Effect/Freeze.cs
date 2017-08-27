using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : SkillEffectBase {

    private void Awake()
    {
        this.skillEffectType = SkillEffectType.Freeze;
    }
}
