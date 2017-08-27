using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMultiple : SkillEffectBase {

    private void Awake()
    {
        this.skillEffectType = SkillEffectType.SpeedMultiple;
    }

    public float percentToMultiply;

    public float GetMultipleValue()
    {
        return (1+this.percentToMultiply/100);
    }
}
