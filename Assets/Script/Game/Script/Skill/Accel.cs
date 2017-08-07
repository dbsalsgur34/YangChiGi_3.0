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
        SkillSoundEffect("SkillEffect_Run", durationTime);

        if (Owner.GetPlayerState().IsBoost)
        {
            yield return new WaitUntil(() => Owner.GetPlayerState().IsBoost == false);
        }
        Owner.GetPlayerState().IsBoost = true;
        Owner.GetPMI().Speed *= 1.5f;
        Owner.GetPMI().TurnSpeed *= 1.5f;

        yield return base.ActivityDuringDurationTime();

        Owner.GetPMI().Speed /= 1.5f;
        Owner.GetPMI().TurnSpeed /= 1.5f;
        Owner.GetPlayerState().IsBoost = false;
    }
}
