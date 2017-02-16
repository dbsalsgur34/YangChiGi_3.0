using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DogState
{
    GO,
    BACK
}

public class Dog : SkillBase {

    

    public DogState DS;
    public List<GameObject> SheepList;

    public float Speed;
    public float mindistance;

    Transform curtransform;
    Transform prevtransform;

    //bool IsbackTohome;

    public override void Awake()
    {
        base.Awake();
        DS = DogState.GO;
    }

    void GoStraight()
    {
        float betangle = Vector3.Angle(Owner.GetComponent<PlayerControlThree>().HQ.transform.position, this.transform.position);
        if (betangle > 90)
        {
            DS = DogState.BACK;
        }
        if (DS == DogState.GO)
        {
            SkillParent.transform.Rotate(new Vector3(-Speed * Time.deltaTime, 0, 0));
        }
        else if (DS == DogState.BACK)
        {
            SkillParent.transform.Rotate(new Vector3(Speed * Time.deltaTime, 0, 0));
        }
    }

    void LeaderSheep()
    {
        for (int i = 0; i < SheepList.Count; i++)
        {
            if (i == 0)
            {
                curtransform = SheepList[i].transform;
                prevtransform = this.transform;
            }
            else
            {
                curtransform = SheepList[i].transform;
                prevtransform = SheepList[i - 1].transform;
            }

            float dis = Vector3.Distance(prevtransform.position, curtransform.position);
            Vector3 newpos = prevtransform.position;

            float T = Time.deltaTime * dis / mindistance * Speed;
            if (T > 0.5f)
                T = 0.5f;
            curtransform.position = Vector3.Slerp(curtransform.position, newpos, T);
            curtransform.rotation = Quaternion.Slerp(curtransform.rotation, prevtransform.rotation, T);
        }
    }

    public void ChangeMaster(GameObject Sheep, GameObject target)
    {
        int index = SheepList.IndexOf(Sheep);

        for (int temp = index; temp <= SheepList.Count - 1; temp++)
        {
            SheepList[temp].GetComponent<SheepControlThree>().Master = target;
            GM.FindAndRemoveAtSheepList(this.SheepList[temp]);
            target.GetComponent<PlayerControlThree>().SheepList.Add(this.SheepList[temp]);
            SheepList[temp].transform.parent = target.GetComponent<PlayerControlThree>().SheepArea.transform;
            SheepList[temp].GetComponent<SheepControlThree>().SetthisLocalPosition();
        }
        SheepList.RemoveRange(index, SheepList.Count - index);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "BronzeSheep" && other.GetComponent<SheepControlThree>().SS != SheepState.HAVEOWNER)
        {
            bool IsthisSheepInList = false;
            foreach (GameObject i in this.SheepList)
            {
                if (other.gameObject == i)
                {
                    IsthisSheepInList = true;
                }
            }
            if (IsthisSheepInList == false)
            {
                other.GetComponent<SheepControlThree>().Master = this.gameObject;
                other.GetComponent<SheepControlThree>().SS = SheepState.HAVEOWNER;
                this.SheepList.Add(other.gameObject);
            }
        }
    }

    public IEnumerator EnterHQ()
    {
        if (SheepList.Count > 0)
        {
            for (int i = SheepList.Count - 1; i >= 0; i--)
            {
                GM.FindAndRemoveAtSheepList(this.SheepList[i]);
                SheepList[i].SetActive(false);
                SheepList.RemoveAt(i);
                Owner.GetComponent<PlayerControlThree>().Score++;
                yield return new WaitForSeconds(0.1f);
            }
            this.gameObject.SetActive(false);
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
        if (SS == SkillState.LAUNCHED && this.Owner != null)
        {
            GoStraight();
        }
        LeaderSheep();
    }


}
