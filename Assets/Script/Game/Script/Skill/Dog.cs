using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DogState
{
    GO,
    BACK
}

[RequireComponent(typeof(DogHerdSheepControl))]

public class Dog : SkillBase {

    private DogState DS;

    public float speed;
    public float mindistance;
    private DogHerdSheepControl dogHerdSheepControl;

    //bool IsbackTohome;

    public override void Awake()
    {
        base.Awake();

        this.dogHerdSheepControl = GetComponent<DogHerdSheepControl>();
    }

    protected override void Start()
    {
        base.Start();
    }

    public DogHerdSheepControl GetDogHerdSheepControl()
    {
        return this.dogHerdSheepControl;
    }

    public override bool SetInstance(PlayerControlThree IO, GameObject ITG)
    {
        dogHerdSheepControl.InitHerdSheepBase(IO, this.speed, false);
        return base.SetInstance(IO, ITG);
    }

    private void GoStraight()
    {
        float betangle = Vector3.Angle(Owner.HQ.transform.position, this.transform.position);
        if (betangle > 90)
        {
            DS = DogState.BACK;
        }
        if (DS == DogState.GO)
        {
            SkillParent.transform.Rotate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        else if (DS == DogState.BACK)
        {
            SkillParent.transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
    }

    protected override IEnumerator ActivityDuringDurationTime()
    {
        DS = DogState.GO;
        SkillSoundEffect("SkillEffect_Dog", 2f);

        return base.ActivityDuringDurationTime();
    }

    private void FixedUpdate()
    {
        if (SS == SkillState.LAUNCHED && this.Owner != null && GameTime.IsTimerStart())
        {
            GoStraight();
        }
    }
}
