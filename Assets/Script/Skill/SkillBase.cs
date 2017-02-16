using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public enum SkillState
{
    ACTIVATED,
    LAUNCHED
}

public class SkillBase : MonoBehaviour {

    protected GameManager GM;
    public GameObject Owner;
    protected GameObject TG;       //Skill의 Target.
    public GameObject SkillParent;
    public float PreCoolTime;
    public SkillState SS;
    public bool IsSkillNeedGuideLine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.Owner)
        {
            SkillAction(other);
        }
    }

    public virtual void SkillAction(Collider other) { }

    public virtual void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        SkillParent = new GameObject("SkillParent");                //create a new gameObject
        transform.parent = SkillParent.transform;                    //make this camera a child of the new gameObject
        SkillParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
    }

    public virtual bool SetInstance(GameObject IO, GameObject ITG)
    {
        this.Owner = IO;
        this.TG = ITG;
        return ((Owner != null && TG != null) ? true : false);
    }

    public virtual void SetPivot(Transform pivot, Transform pivotRotation, float angle)
    {
        this.SkillParent.transform.position = pivot.position;

        Quaternion temprotationone = Quaternion.AngleAxis(angle,pivot.up);
        Quaternion temprotationtwo = Quaternion.Euler(pivotRotation.eulerAngles.z, pivotRotation.eulerAngles.z + temprotationone.eulerAngles.y,0);
        this.SkillParent.transform.rotation = temprotationtwo;
    }

    public virtual float ShowPreCooltime()
    {
        return PreCoolTime;
    }

    public virtual bool ShowIsSkillNeedGuideLine()
    {
        return this.IsSkillNeedGuideLine;
    }
}
