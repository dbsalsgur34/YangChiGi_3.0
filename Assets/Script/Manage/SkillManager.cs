using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public SkillDataBase SkillDB;
    public List<GameButtonEvent> SkillButtonList;

    private void Start()
    {
        //스킬 관련 초기화.
        SkillDB = this.gameObject.GetComponent<SkillDataBase>();
    }

    public void UsingSkill(int SkillNumber, GameObject Owner, GameObject Target, Transform Pivot, Quaternion Way)
    {
        GameObject ActivatedSkill = Instantiate(SkillDB.SkillPrefab[SkillNumber]);
        SkillBase ActivatedSkillInit = ActivatedSkill.GetComponent<SkillBase>();
        ActivatedSkillInit.SetInstance(Owner, Target);
        ActivatedSkillInit.SetPivot(Pivot,Way);
    }
}
