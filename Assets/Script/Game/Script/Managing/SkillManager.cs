﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    SkillDataBase SkillDB;


    public List<DragAndDropItem> SkillPanelQueue;
    int[] SkillindexArray;
    int index;

    private void Start()
    {
        //스킬 관련 초기화.
        SkillDB = PlayManage.Instance.gameObject.GetComponent<SkillDataBase>();
        SkillindexArray = new int[SkillDB.SkillPrefab.Count - 1];
        index = 0;
    }

    public void UsingSkill(int SkillNumber, GameObject Owner, GameObject Target, Transform Pivot, Transform Pivotrotation,float angle, Vector3 skillVector)
    {
        GameObject ActivatedSkill = Instantiate(SkillDB.SkillPrefab[SkillNumber]);
        SkillBase ActivatedSkillInit = ActivatedSkill.GetComponent<SkillBase>();
        ActivatedSkillInit.SetInstance(Owner, Target);
        ActivatedSkillInit.SetPivot(Pivot,Pivotrotation,angle,skillVector);
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
            SkillPanelQueue[0].SetInstance(i,SkillDB.SkillPrefab[i].GetComponent<SkillBase>().ShowIsSkillNeedGuideLine());
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
        while (SkillPanelQueue.Count != 0 && GameUIManager.GUIMInstance.GetIsTimerStart())
        {
            SetSkillPanelSkill();
        }
    }
}
