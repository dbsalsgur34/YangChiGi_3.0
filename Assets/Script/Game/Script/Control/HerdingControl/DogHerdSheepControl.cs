using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHerdSheepControl : HerdSheepBase {

    public override void AddSheepList(SheepControlThree Sheep)
    {
        base.AddSheepList(Sheep);
    }

    public override void ChangeMasterToTargetOwner(SheepControlThree Sheep, HerdSheepBase target)
    {
        base.ChangeMasterToTargetOwner(Sheep, target);
        int index = herdSheepList.IndexOf(Sheep);

        for (int temp = index; temp <= herdSheepList.Count - 1; temp++)
        {
            herdSheepList[temp].Follower = target;
            ManagerHandler.Instance.GameManager().FindAndRemoveAtSheepList(this.herdSheepList[temp].gameObject);
            target.AddSheepList(this.herdSheepList[temp]);
        }
        herdSheepList.RemoveRange(index, herdSheepList.Count - index);
    }

    protected override void HerdingSheep()
    {
        base.HerdingSheep();
        for (int i = 0; i < herdSheepList.Count; i++)
        {
            if (i == 0)
            {
                curTransform = herdSheepList[i].transform;
                prevTransform = this.transform;
            }
            else
            {
                curTransform = herdSheepList[i].transform;
                prevTransform = herdSheepList[i - 1].transform;
            }

            float dis = Vector3.Distance(prevTransform.position, curTransform.position);
            Vector3 newpos = prevTransform.position;

            float T = Time.deltaTime * dis / mindistance * speed;
            if (T > 0.5f)
                T = 0.5f;
            curTransform.position = Vector3.Slerp(curTransform.position, newpos, T);
            curTransform.rotation = Quaternion.Slerp(curTransform.rotation, prevTransform.rotation, T);
        }
    }

    public override void InitHerdSheepBase(PlayerControlThree Owner, float speed, bool takeOverPermitSet)
    {
        base.InitHerdSheepBase(Owner, speed, takeOverPermitSet);
    }
}
