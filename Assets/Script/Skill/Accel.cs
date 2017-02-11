using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accel : SkillBase {

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(AccelAction());
	}

    public override bool SetInstance(GameObject IO, GameObject ITG)
    {
        return base.SetInstance(IO, ITG);
    }

    public override bool FindSkillNeedCameraFix()
    {
        return base.FindSkillNeedCameraFix();
    }

    IEnumerator AccelAction()
    {
        Owner.GetComponent<PlayerControlThree>().speed *= 1.5f;
        yield return new WaitForSeconds(5f);
        Owner.GetComponent<PlayerControlThree>().speed /= 1.5f;
        this.gameObject.SetActive(false);
    }
}
