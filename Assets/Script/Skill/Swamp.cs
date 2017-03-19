using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swamp : SkillBase {

    public float slow = 3.3f;
    float Iconrotation = 0;
    public List<PlayerControlThree> colliderList;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(SwampLifeTime());
    }

    IEnumerator SwampLifeTime()
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.GetComponent<Collider>().enabled = false;
        if (colliderList.Count != 0)
        {
            foreach (PlayerControlThree i in colliderList)
            {
                i.speed *= slow;
            }
        }
        SkillParent.gameObject.SetActive(false);
    }

    void TargetIconRotate()
    {
        Iconrotation += 10f * Time.deltaTime;
        
        this.transform.localRotation = Quaternion.Euler(90, Iconrotation, 0);
    }

    Quaternion targetRotation(Vector3 target)
    {
        return Quaternion.FromToRotation(this.transform.position, target) * SkillParent.transform.rotation; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            colliderList.Add(other.GetComponent<PlayerControlThree>());
            other.gameObject.GetComponent<PlayerControlThree>().speed /= slow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Head")
        {
            other.gameObject.GetComponent<PlayerControlThree>().speed *= slow;
            colliderList.Remove(other.GetComponent<PlayerControlThree>());
        }
    }

    public override bool SetInstance(GameObject IO, GameObject ITG)
    {
        return base.SetInstance(IO, ITG);
    }

    public override void SetPivot(Transform pivot, Transform pivotRotation, float angle)
    {
        base.SetPivot(pivot, pivotRotation, angle);
        SkillParent.transform.rotation = targetRotation(GM.hitVector);
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
        TargetIconRotate();
    }
}
