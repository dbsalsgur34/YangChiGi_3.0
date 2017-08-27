using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private struct EffectedStruct
    {
        public SkillEffectBase targetEffect { get; private set; }
        public float inputTime { get; private set; }

        public EffectedStruct(SkillEffectBase targetEffect,float inputTime)
        {
            this.targetEffect = targetEffect;
            this.inputTime = inputTime;
        }
    }

    public bool InHQ { get; set; }

    public float MultiplyValue { get; private set; }
    public bool IsFreeze { get; private set; }
    public bool IsKnockBack { get; private set; }

    private List<EffectedStruct> effectedList;

    private void Awake()
    {
        InHQ = true;

        IsFreeze = false;
        IsKnockBack = false;
        effectedList = new List<EffectedStruct>();
    }

    public void SetupEffectList(List<SkillEffectBase> targetList)
    {
        foreach (SkillEffectBase effect in targetList)
        {
            effectedList.Add(new EffectedStruct(effect,ManagerHandler.Instance.GameTime().GetTime()));
        }
    }

    public void CheckEffectList()
    {
        float tempValue = 1;
        bool tempIsFreeze = false;
        bool tempIsKnockBack = false;
        foreach (EffectedStruct effect in effectedList)
        {
            if (effect.targetEffect.GetType().Equals(typeof(SpeedMultiple)))
            {
                tempValue *= (effect.targetEffect as SpeedMultiple).GetMultipleValue();
            }
            else if (effect.targetEffect.GetType().Equals(typeof(Freeze)))
            {
                IsFreeze = true;
            }
            //else if(effect.targetEffect.GetType().Equals(typeof(KnockBack))) 넉백상태일시 구현할 예정.

            float passedTime = effect.inputTime + effect.targetEffect.effectDuration;
            if (ManagerHandler.Instance.GameTime().GetTime() > passedTime)
            {
                effectedList.Remove(effect);
            }
        }
        MultiplyValue = tempValue;
        IsFreeze = tempIsFreeze;
        IsKnockBack = tempIsKnockBack;
    }

}
