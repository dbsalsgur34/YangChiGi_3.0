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

    private DogState DS;
    private List<SheepControlThree> SheepList;
    public float Speed;
    public float mindistance;

    Transform curtransform;
    Transform prevtransform;

    //bool IsbackTohome;

    public override void Awake()
    {
        base.Awake();
        DS = DogState.GO;
        ChangeSkillStateLaunched();
        SheepList = new List<SheepControlThree>();
    }

    private void GoStraight()
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

    private void LeaderSheep()
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

    public void ChangeMaster(SheepControlThree Sheep, PlayerControlThree target)
    {
        int index = SheepList.IndexOf(Sheep);

        for (int temp = index; temp <= SheepList.Count - 1; temp++)
        {
            SheepList[temp].Master = target.gameObject;
            ManagerHandler.Instance.GameManager().FindAndRemoveAtSheepList(this.SheepList[temp].gameObject);
            target.SheepList.Add(this.SheepList[temp].gameObject);
            SheepList[temp].transform.parent = target.SheepArea.transform;
            SheepList[temp].GetComponent<SheepControlThree>().SetthisLocalPosition();
        }
        SheepList.RemoveRange(index, SheepList.Count - index);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "BronzeSheep" && other.GetComponent<SheepControlThree>().GetSheepState() != SheepState.HAVEOWNER)
        {
            bool IsthisSheepInList = false;
            foreach (SheepControlThree i in this.SheepList)
            {
                if (other.gameObject == i.gameObject)
                {
                    IsthisSheepInList = true;
                }
            }
            if (IsthisSheepInList == false)
            {
                SheepControlThree SCT = other.GetComponent<SheepControlThree>();
                SCT.Master = this.gameObject;
                SCT.CheckSheepState();
                this.SheepList.Add(SCT);
            }
        }
    }

    public IEnumerator EnterHQ()
    {
        if (SheepList.Count > 0)
        {
            for (int i = SheepList.Count - 1; i >= 0; i--)
            {
                ManagerHandler.Instance.GameManager().FindAndRemoveAtSheepList(this.SheepList[i].gameObject);
                SheepList[i].gameObject.SetActive(false);
                SheepList.RemoveAt(i);
                Owner.GetComponent<PlayerControlThree>().Score++;
                yield return new WaitForSeconds(0.1f);
            }
            this.gameObject.SetActive(false);
        }
    }

    public DogState GetDogState()
    {
        return this.DS;
    }

    public int GetDogSheepCount()
    {
        return this.SheepList.Count;
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

    private void FixedUpdate()
    {
        if (SS == SkillState.LAUNCHED && this.Owner != null)
        {
            GoStraight();
        }
        LeaderSheep();
    }

    

}
