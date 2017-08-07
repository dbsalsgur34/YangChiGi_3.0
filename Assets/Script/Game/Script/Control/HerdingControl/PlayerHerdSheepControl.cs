using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHerdSheepControl : HerdSheepBase {

    private GameObject SheepArea;

    public override void InitHerdSheepBase(PlayerControlThree Owner, float speed, bool takeOverPermitSet)
    {
        base.InitHerdSheepBase(Owner, speed, takeOverPermitSet);
        SheepArea = new GameObject("SheepArea");
        SheepArea.transform.position = this.transform.position;
        prevTransform = this.transform;
        curTransform = SheepArea.transform;
        mindistance = 15f;
    }

    public override void ChangeMasterToTargetOwner(SheepControlThree Sheep, HerdSheepBase target)
    {
        base.ChangeMasterToTargetOwner(Sheep, target);
    }

    protected override void HerdingSheep()
    {
        base.HerdingSheep();
        float dis = Vector3.Distance(prevTransform.position, curTransform.position);
        Vector3 newpos = prevTransform.position;

        float divisor = Mathf.Sqrt(mindistance * speed);
        if (divisor == 0) { divisor = 1f; }
        float T = Time.fixedDeltaTime * dis / divisor;
        if (T > 0.5f)
            T = 0.5f;
        curTransform.position = Vector3.Slerp(curTransform.position, newpos, T);
        curTransform.rotation = Quaternion.Slerp(curTransform.rotation, prevTransform.rotation, T);
    }

    public override void AddSheepList(SheepControlThree Sheep)
    {
        base.AddSheepList(Sheep);
        SetSheepLocalPosition(Sheep.transform, this.SheepArea.transform);
    }

    private void SetSheepLocalPosition(Transform target, Transform LocalParent)
    {
        Vector2 Circleposition = Random.insideUnitCircle * 3;
        target.parent = LocalParent;
        target.transform.localPosition = new Vector3(Circleposition.x, 0, Circleposition.y);
        target.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

}
