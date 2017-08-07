using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public enum SkillState
{
    ACTIVATED,
    LAUNCHED
}
[RequireComponent(typeof(AudioSource))]
public abstract class SkillBase : MonoBehaviour {

    protected PlayerControlThree Owner;
    protected GameObject TG;       //Skill의 Target.
    protected GameObject SkillParent;
    public float PreCoolTime;
    public float waitTime = 5f;
    public float durationTime = 5f;
    public bool IsSkillNeedGuideLine;
    public int requiredLevel;

    protected SkillState SS;

    private AudioSource skillAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.Owner.gameObject)
        {
            CollideSkillAction(other);
        }
    }

    public virtual void CollideSkillAction(Collider other) { }

    public virtual void Awake()
    {
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        SkillParent = new GameObject("SkillParent");                //create a new gameObject
        this.transform.parent = SkillParent.transform;                    //make this camera a child of the new gameObject
        SkillParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
        skillAudio = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        StartSkillLifeCycle();
    }

    public virtual bool SetInstance(PlayerControlThree IO, GameObject ITG)
    {
        this.Owner = IO;
        this.TG = ITG;
        return ((Owner != null && TG != null) ? true : false);
    }

    public virtual void SetPivot(Transform pivot, Transform pivotRotation, float angle, Vector3 skillVector)
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

    public virtual bool GetIsSkillNeedGuideLine()
    {
        return this.IsSkillNeedGuideLine;
    }

    public virtual float GetSkillCoolTime()
    {
        return this.PreCoolTime;
    }

    public bool AreYouMyMaster(PlayerControlThree target)
    {
        return (target.Equals(this.Owner)) ? true : false;
    }

    public int GetRequiredLevel() {
        return this.requiredLevel;
    }

    protected void ChangeSkillStateLaunched()
    {
        this.SS = SkillState.LAUNCHED;
    }

    public SkillState GetSkillState()
    {
        return this.SS;
    }

    protected void SkillSoundEffect(string clipName,float playTime)
    {
        AudioManager.Instance.PlayEffectSoundByIndivisualAudioSource(skillAudio, playTime, clipName);
    }

    protected void SkillSoundEffect(string clipName, float playTime, float volume)
    {
        AudioManager.Instance.PlayEffectSoundByIndivisualAudioSource(skillAudio, playTime, volume, clipName);
    }

    protected void StartSkillLifeCycle()
    {
        StartCoroutine(SkillLifeCycle());
    }

    private IEnumerator SkillLifeCycle()
    {
        yield return StartCoroutine(ActivityDuringWaitTime());
        yield return StartCoroutine(ActivityDuringDurationTime());
        SkillParent.SetActive(false);
    }

    protected virtual IEnumerator ActivityDuringWaitTime()
    {
        yield return new WaitForSeconds(waitTime);
    }

    protected virtual IEnumerator ActivityDuringDurationTime()
    {
        ChangeSkillStateLaunched();
        yield return new WaitForSeconds(durationTime);
    }
}

