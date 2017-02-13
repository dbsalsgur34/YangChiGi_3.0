using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    SkillDataBase SkillDB;
    GameManager GM;
    List<GameButtonEvent> SkillButtonList;
    public List<DragAndDropItem> SkillPanelQueue;
    int[] SkillindexArray;
    int index;

    private void Start()
    {
        //스킬 관련 초기화.
        SkillDB = this.gameObject.GetComponent<SkillDataBase>();
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        SkillindexArray = new int[SkillDB.SkillPrefab.Count - 1];
        index = 0;
    }

    public void UsingSkill(int SkillNumber, GameObject Owner, GameObject Target, Transform Pivot, Quaternion Way)
    {
        GameObject ActivatedSkill = Instantiate(SkillDB.SkillPrefab[SkillNumber]);
        SkillBase ActivatedSkillInit = ActivatedSkill.GetComponent<SkillBase>();
        ActivatedSkillInit.SetInstance(Owner, Target);
        ActivatedSkillInit.SetPivot(Pivot,Way);
    }

    public void SetSkillPanelQueue(DragAndDropItem item)
    {
        SkillPanelQueue.Add(item);
    }

    public void SetSkillPanelSkill()
    {
        if (SkillPanelQueue[0] != null)
        {
            int i = SkillDB.SkillIndexList[index];
            SkillPanelQueue[0].num = i;
            StartCoroutine(SkillPanelQueue[0].SkillDelay(SkillDB.SkillPrefab[i].GetComponent<SkillBase>().ShowPreCooltime(),SkillDB.SkillIcon[0], SkillDB.SkillIcon[i]));
            SkillPanelQueue.RemoveAt(0);
            if (index < SkillDB.SkillIndexList.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
        }
    }

    void Update()
    {
        while (SkillPanelQueue.Count != 0 && GM.TimerStart)
        {
            SetSkillPanelSkill();
        }
    }
}
