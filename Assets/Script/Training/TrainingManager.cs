using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : ManagerBase
{
    public DragAndDropCell_Training[] skillSetPanel;
    public DragAndDropCell_Training[] skillListPanel;
    private SkillDataBase SkillDB;
    public override void Awake()
    {
        base.Awake();
        skillListPanel = GameObject.FindGameObjectWithTag("SkillListPanel").GetComponentsInChildren<DragAndDropCell_Training>();
        skillSetPanel = GameObject.FindGameObjectWithTag("SkillSetPanel").GetComponentsInChildren<DragAndDropCell_Training>();
    }

    public override void Start()
    {
        base.Start();
        SkillDB = PlayManage.Instance.GetSkillDataBase();
        InitSkillListPanel();
        InitSkillSetPanel();
    }

    private void InitSkillListPanel()
    {
        for (int i = 0; i < skillListPanel.Length; i++)
        {
            try
            {

                DragAndDropItem_Training myitem = skillListPanel[i].GetComponentInChildren<DragAndDropItem_Training>();
                int skillRequireLevel = SkillDB.GetSkillPrefab()[i + 1].GetComponent<SkillBase>().GetRequiredLevel();
                if (skillRequireLevel <= (PlayManage.Instance.GetPlayerLevel()))
                {
                    myitem.gameObject.GetComponent<Image>().sprite = SkillDB.GetSkillIcon(i + 1);
                    myitem.IndexNum = (i + 1);
                    myitem.SetItemCanDrag(true);
                }
                else
                {
                    myitem.gameObject.GetComponent<Image>().sprite = GetPadLock();
                    myitem.SetItemCanDrag(false);
                    myitem.ShowRequiredLevel(skillRequireLevel);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private void InitSkillSetPanel()
    {
        for(int i = 0; i<4; i++)
        {
            try
            {
                skillSetPanel[i].SetCelltoSkillSetPanel(i);
                DragAndDropItem_Training myitem = skillSetPanel[i].GetComponentInChildren<DragAndDropItem_Training>();
                int skillIndex = PlayManage.Instance.GetSkillPreSet(i);
                myitem.gameObject.GetComponent<Image>().sprite = SkillDB.GetSkillIcon(skillIndex);
                myitem.IndexNum = (skillIndex);
                myitem.SetItemCanDrag(true);

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private Sprite GetPadLock()
    {
        return Resources.Load<Sprite>("Image/Resource/Button/Black/SkillIcon/padlock");
    }

    private void SaveSkillSet()
    {
        foreach (DragAndDropCell_Training cell in skillSetPanel)
        {
            PlayManage.Instance.SetSkillPreSet(cell.cellNumber,cell.GetComponentInChildren<DragAndDropItem_Training>().IndexNum);
        }
    }

    private void Update()
    {
        SaveSkillSet();
    }
}
