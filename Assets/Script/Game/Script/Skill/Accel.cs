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

    public override bool GetIsSkillNeedGuideLine()
    {
        return base.GetIsSkillNeedGuideLine();
    }

    public void ColliderSkillAction(Collider other) { Debug.Log("충돌하는 스킬이 아니라 하는게 없음"); }

    IEnumerator AccelAction()
    {
        SkillSoundEffect("SkillEffect_Run", duration);
        PlayerControlThree OwnerPCT = Owner.GetComponent<PlayerControlThree>();
        if (OwnerPCT.GetPlayerState().IsBoost)
        {
            yield return new WaitUntil(() => OwnerPCT.GetPlayerState().IsBoost == false);
        }
        OwnerPCT.GetPlayerState().IsBoost = true;
        OwnerPCT.GetPMI().Speed *= 1.5f;
        OwnerPCT.GetPMI().TurnSpeed *= 1.5f;
        yield return new WaitForSeconds(duration);
        OwnerPCT.GetPMI().Speed /= 1.5f;
        OwnerPCT.GetPMI().TurnSpeed /= 1.5f;
        OwnerPCT.GetPlayerState().IsBoost = false;
        this.gameObject.SetActive(false);
    }
}
