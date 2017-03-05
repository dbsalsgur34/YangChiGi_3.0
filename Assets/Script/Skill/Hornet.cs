using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : SkillBase {

    public float Speed;
    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        StartCoroutine(HornetLife());
        SS = SkillState.ACTIVATED;
	}

    void HornetAction(GameObject Target)
    {
        float slowspeed = Speed / 10;
        float Gospeed;

        if (Target != null && !Target.GetComponent<PlayerControlThree>().InHQ )
        {
            Gospeed = Speed;
        }
        else
        {
            Gospeed = slowspeed;
        }
        
        if (SS == SkillState.LAUNCHED)
        {
            this.SkillParent.transform.rotation *= (GoStraight(Gospeed) * TurnToTarget());
        }
    }

    Quaternion TurnToTarget()
    {
        float angle;
        Vector3 PO = this.gameObject.transform.position;
        Vector3 TO = TG.transform.position;
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(this.gameObject.transform.right, PTVector);
        return Quaternion.AngleAxis(angle, SkillParent.transform.up); 
    }

    Quaternion GoStraight(float SP)
    {
        return Quaternion.Euler(new Vector3(-SP * Time.deltaTime, 0, 0));
    }

    IEnumerator HornetLife()
    {
        yield return new WaitForSeconds(5f);
        SS = SkillState.LAUNCHED;
        yield return new WaitForSeconds(10f);
        SkillParent.SetActive(false);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "Head" && other.gameObject == TG)
        {
            if (!other.gameObject.GetComponent<PlayerControlThree>().InHQ)
            {
                other.gameObject.GetComponent<PlayerControlThree>().StartCoroutine(other.gameObject.GetComponent<PlayerControlThree>().HornetAttack());
            }
            SkillParent.SetActive(false);
        }
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

    private void Update()
    {
        HornetAction(TG);
    }
}
