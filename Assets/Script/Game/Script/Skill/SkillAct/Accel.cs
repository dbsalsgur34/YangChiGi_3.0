using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : SkillBase {

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
	}

    protected override void Start()
    {
        base.Start();
        
    }

    protected override IEnumerator ActivityDuringDurationTime()
    {
        Owner.GetPlayerState().SetupEffectList(this.skillEffectList);
        return base.ActivityDuringDurationTime();
    }

}
