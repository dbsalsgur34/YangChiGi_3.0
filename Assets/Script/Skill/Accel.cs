﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : SkillBase {

    public float accelTime = 5f;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
	}

    private void Start()
    {
        StartCoroutine(AccelAction());
    }

    public override bool SetInstance(GameObject IO, GameObject ITG)
    {
        return base.SetInstance(IO, ITG);
    }

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle)
    {
        base.SetPivot(pivot, pivotRotation, angle);
    }

    public override float ShowPreCooltime()
    {
        return base.ShowPreCooltime();
    }

    public override bool ShowIsSkillNeedGuideLine()
    {
        return base.ShowIsSkillNeedGuideLine();
    }

    IEnumerator AccelAction()
    {
        if (Owner.GetComponent<PlayerControlThree>().IsBoost)
        {
            yield return new WaitUntil(() => Owner.GetComponent<PlayerControlThree>().IsBoost == false);
        }
        Owner.GetComponent<PlayerControlThree>().IsBoost = true;
        Owner.GetComponent<PlayerControlThree>().speed *= 1.5f;
        Owner.GetComponent<PlayerControlThree>().turnspeed *= 1.5f;
        yield return new WaitForSeconds(accelTime);
        Owner.GetComponent<PlayerControlThree>().speed /= 1.5f;
        Owner.GetComponent<PlayerControlThree>().turnspeed /= 1.5f;
        Owner.GetComponent<PlayerControlThree>().IsBoost = false;
        this.gameObject.SetActive(false);
    }
}
