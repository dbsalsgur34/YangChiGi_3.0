using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : SkillBase {

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

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle,Vector3 skillVector)
    {
        base.SetPivot(pivot, pivotRotation, angle,skillVector);
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
        PlayerControlThree OwnerPCT = Owner.GetComponent<PlayerControlThree>();
        if (OwnerPCT.IsBoost)
        {
            yield return new WaitUntil(() => OwnerPCT.IsBoost == false);
        }
        OwnerPCT.IsBoost = true;
        OwnerPCT.GetPMI().Speed *= 1.5f;
        OwnerPCT.GetPMI().TurnSpeed *= 1.5f;
        yield return new WaitForSeconds(duration);
        OwnerPCT.GetPMI().Speed /= 1.5f;
        OwnerPCT.GetPMI().TurnSpeed /= 1.5f;
        OwnerPCT.IsBoost = false;
        this.gameObject.SetActive(false);
    }
}
