using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdSheepBase : MonoBehaviour, BootNeedInterFace {

    public BootState bootState { get; private set; }

    public float mindistance;
    protected float speed;
    private bool takeOverPermit;
    private PlayerControlThree Owner;
    public List<SheepControlThree> herdSheepList;
    protected Transform curTransform;
    protected Transform prevTransform;

    public void InitialState()
    {
        bootState = BootState.NeedBoot;
    }

    private void Awake()
    {
        InitialState();
    }

    public bool isTakeOverPermit()
    {
        return this.takeOverPermit;
    }

    public virtual void InitHerdSheepBase(PlayerControlThree Owner, float speed, bool takeOverPermitSet)
    {
        this.speed = speed;
        this.Owner = Owner;
        herdSheepList = new List<SheepControlThree>();
        bootState = BootState.Complete;

        takeOverPermit = takeOverPermitSet;
    }

    protected virtual void HerdingSheep()
    {
        
    }

    public PlayerControlThree GetOwner()
    {
        return this.Owner;
    }

    private void Update()
    {
        if (GameTime.IsTimerStart() && bootState.Equals(BootState.Complete))
        {
            HerdingSheep();
        }
    }

    public virtual void ChangeMasterToTargetOwner(SheepControlThree Sheep, HerdSheepBase target)
    {
        herdSheepList.Remove(Sheep);
        Sheep.Follower = target;
        Sheep.GetSpriteRenderer().color = target.GetOwner().GetSymbolColor();
        target.AddSheepList(Sheep);
    }

    public virtual void AddSheepList(SheepControlThree Sheep)
    {
            // 양을 추가시키고, HerdSheepBase의 Owner의 상징색으로 양의 색깔을 바꾼다.
            this.herdSheepList.Add(Sheep);
            Sheep.GetSpriteRenderer().color = Owner.GetSymbolColor();
    }

    public int GetHerdSheepCount()
    {
        return this.herdSheepList.Count;
    }

    public virtual IEnumerator MoveAllSheepToTarget(HerdSheepBase target)
    {

        if (herdSheepList.Count > 0)
        {
            for (int i = herdSheepList.Count - 1; i >= 0; i--)
            {
                SheepControlThree Sheep = herdSheepList[i];
                herdSheepList.Remove(Sheep);
                Sheep.Follower = target;
                Sheep.GetSpriteRenderer().color = target.GetOwner().GetSymbolColor();
                target.AddSheepList(Sheep);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

}
