using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : SkillBase {

    public float speed = 20;
    public float maxDegree = 10;
    public float freezeTime = 3f;

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(HornetLife());
        SS = SkillState.ACTIVATED;
	}

    private void HornetAction(GameObject Target)
    {
        float slowSpeed = speed / 10;
        float goSpeed;

        if (Target != null && !Target.GetComponent<PlayerControlThree>().GetPlayerState().InHQ )
        {
            goSpeed = speed;
        }
        else
        {
            goSpeed = slowSpeed;
        }
        
        if (SS == SkillState.LAUNCHED)
        {
            this.SkillParent.transform.rotation = TurnToTarget();
            this.SkillParent.transform.rotation *= GoStraight(goSpeed);
        }
    }

    private Quaternion TurnToTarget()
    {
        float angle;
        Vector3 PO = this.gameObject.transform.position;
        Vector3 TO = TG.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(this.gameObject.transform.right, PTVector);
        Quaternion AA = Quaternion.AngleAxis(angle, SkillParent.transform.up) * this.SkillParent.transform.rotation;
        return Quaternion.Slerp(this.SkillParent.transform.rotation, AA, maxDegree * Time.deltaTime);
    }

    private Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(SP * Time.deltaTime, 0, 0));
    }

    private IEnumerator HornetLife()
    {
        yield return new WaitForSeconds(waitTime);
        ChangeSkillStateLaunched();
        yield return new WaitForSeconds(duration);
        SkillParent.SetActive(false);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "Head" && other.gameObject == TG)
        {
            PlayerControlThree otherPCT = other.GetComponent<PlayerControlThree>();
            if (!otherPCT.GetPlayerState().InHQ)
            {
                if (otherPCT.GetPlayerState().IsFreeze)
                {
                    SkillParent.SetActive(false);
                    return;
                }
                otherPCT.StartCoroutine(otherPCT.PlayerFreeze(this.freezeTime,100));
            }
            SkillParent.SetActive(false);
        }
    }

    public override bool SetInstance(GameObject IO, GameObject ITG)
    {
        return base.SetInstance(IO, ITG);
    }

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle, Vector3 skillVector)
    {
        base.SetPivot(pivot, pivotRotation, angle, skillVector);
    }

    public override float ShowPreCooltime()
    {
        return base.ShowPreCooltime();
    }

    public override bool GetIsSkillNeedGuideLine()
    {
        return base.GetIsSkillNeedGuideLine();
    }

    private void Update()
    {
        HornetAction(TG);
    }
}
