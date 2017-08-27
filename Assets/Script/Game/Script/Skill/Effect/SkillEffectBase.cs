using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillEffectType
{
    SpeedMultiple,
    Freeze,
    KnockBack
}

public class SkillEffectBase : MonoBehaviour {

    public SkillEffectType skillEffectType{ get; protected set; }
    public string effectName;
    public float effectDuration;

    public float GetEffectDuration()
    {
        return this.effectDuration;
    }

    public string GetEffectName()
    {
        if (effectName.Equals(null))
        {
            return this.gameObject.name;
        }
        else
        {
            return this.effectName;
        }
    }
}
