using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBaseInterface : MonoBehaviour {

    protected abstract void SkillSoundEffect(AudioClip soundEffect);

    public abstract void SkillAction(Collider other);
}
