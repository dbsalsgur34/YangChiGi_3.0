using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDataBase : MonoBehaviour {

    public List<GameObject> skillPrefab;
    public List<Sprite> skillIcon;

    public List<GameObject> GetSkillPrefab()
    {
        return this.skillPrefab;
    }

    public Sprite GetSkillIcon(int index)
    {
        return this.skillIcon[index];
    }

    public bool GetIsGuideLineNeed(int index)
    {
        return skillPrefab[index].GetComponent<SkillBase>().GetIsSkillNeedGuideLine();
    }

    public float GetSkillCoolTime(int index)
    {
        return skillPrefab[index].GetComponent<SkillBase>().GetSkillCoolTime();
    }
}
