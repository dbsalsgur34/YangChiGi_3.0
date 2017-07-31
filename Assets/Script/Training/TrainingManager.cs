using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : ManagerBase
{
    public static TrainingManager TMInstance;
    private List<DragAndDropCell_PreSet> skillSetPanel;
    private List<DragAndDropCell_Training> skillListPanel;
    private SkillDataBase skillDB;
    private ErrorMessageWindow errorMessageWindow;
    private Sprite selectSquareSprite;
    private Color nullColor;
    public int[] preSetList;

    protected new void Awake()
    {
        TMInstance = this;
        skillListPanel = new List<DragAndDropCell_Training>(GameObject.FindGameObjectWithTag("SkillListPanel").GetComponentsInChildren<DragAndDropCell_Training>());
        skillSetPanel = new List<DragAndDropCell_PreSet>(GameObject.FindGameObjectWithTag("SkillSetPanel").GetComponentsInChildren<DragAndDropCell_PreSet>());
    }

    protected override void Start()
    {
        base.Start();
        skillDB = GameObject.FindGameObjectWithTag("SkillDataBase").GetComponent<SkillDataBase>();
        errorMessageWindow = GameObject.Find("ErrorMessage").GetComponent<ErrorMessageWindow>();
        errorMessageWindow.InitErrorMessageWindow();
        nullColor = new Vector4(1, 1, 1, 0.4f);
        InitSelectSquare();
        InitSkillListPanel();
        InitSkillSetPanel();
    }

    private void InitSkillListPanel()
    {
        for (int i = 0; i < skillListPanel.Count; i++)
        {
            DragAndDropItem_Training myitem = skillListPanel[i].GetComponentInChildren<DragAndDropItem_Training>();
            int skillRequireLevel = skillDB.GetSkillPrefab()[i].GetComponent<SkillBase>().GetRequiredLevel();
            if (skillRequireLevel <= (PlayManage.Instance.GetPlayerLevel()))
            {
                myitem.gameObject.GetComponent<Image>().sprite = skillDB.GetSkillIcon(i);
                myitem.IndexNum = (i);
                myitem.SetItemCanDrag(true);
            }
            else
            {
                myitem.gameObject.GetComponent<Image>().sprite = GetPadLock();
                myitem.SetItemCanDrag(false);
                ShowRequiredLevel(skillRequireLevel, myitem.transform);
            }
        }
    }

    private void ShowRequiredLevel(int requiredLevel, Transform targetTransform)
    {
        Text text = (Instantiate(Resources.Load("Prefab/ETC/LevelText"), targetTransform) as GameObject).GetComponent<Text>();
        text.text = "Lv." + requiredLevel.ToString();
    }

    private void InitSkillSetPanel()
    {
        string[] tempList = PlayManage.Instance.SkillPreSet.Split(',');
        int skillIndex;
        preSetList = new int[4];
        for (int i = 0; i < 4; i++)
        {
            skillSetPanel[i].SetSkillSetPanel(i);
            DragAndDropItem_Training myitem = skillSetPanel[i].GetComponentInChildren<DragAndDropItem_Training>();
            skillIndex = int.Parse(tempList[i]);
            preSetList[i] = skillIndex;
            myitem.gameObject.GetComponent<Image>().sprite = skillDB.GetSkillIcon(skillIndex);
            myitem.IndexNum = (skillIndex);
            myitem.SetItemCanDrag(true);
        }
        MarkSelectSquare(this.skillListPanel, this.preSetList, this.selectSquareSprite);
    }

    private void InitSelectSquare()
    {
        this.selectSquareSprite = Resources.Load<Sprite>("Image/Resource/UI/Square");
    }

    private Sprite GetPadLock()
    {
        return Resources.Load<Sprite>("Image/Resource/Button/Black/SkillIcon/padlock");
    }

    private GameObject GetSelectSquare()
    {
        return Resources.Load<GameObject>("Prefab/ETC/SelectSquare");
    }

    public void DragEndAction(int? sourceCellNum, int? descCellNum, int skillNum)
    {
        if (sourceCellNum.Equals(null)) //패널->프리셋
        {
            foreach (DragAndDropCell_PreSet i in skillSetPanel)  //중복검사
            {
                int index = (int)i.GetCellNumber();
                if (!i.transform.childCount.Equals(0))
                {
                    if (i.GetItem().IndexNum.Equals(skillNum) && !i.GetCellNumber().Equals(descCellNum))
                    {
                        i.RemoveItem();
                        preSetList[index] = -1;
                    }
                    else
                    {
                        int targetIndex = i.GetItem().IndexNum;
                        preSetList[index] = targetIndex;
                    }
                }
            }
            AudioManager.Instance.PlayOneShotEffectClipByName("PanelMove");
        }
        else                                   //프리셋->프리셋
        {
            foreach (DragAndDropCell_PreSet i in skillSetPanel)
            {
                int index = (int)i.GetCellNumber();
                if (i.transform.childCount.Equals(0))
                {
                    preSetList[index] = -1;
                }
                else
                {
                    int targetIndex = i.GetItem().IndexNum;
                    preSetList[index] = targetIndex;
                }
            }
        }
        MarkSelectSquare(this.skillListPanel, this.preSetList, this.selectSquareSprite);
    }

    private void MarkSelectSquare(List<DragAndDropCell_Training> skillListPanel,int[] preSetList, Sprite selectSquareIcon)
    {
        foreach(DragAndDropCell_Training i in skillListPanel)
        {
            Image target = i.GetComponent<Image>();
            target.color = nullColor;
            target.sprite = null;            
        }

        for (int i = 0; i < 4; i++)
        {
            if (preSetList[i] != -1)
            {
                Image target = skillListPanel[preSetList[i]].GetComponent<Image>();
                target.color = Color.white;
                target.sprite = selectSquareIcon;
            }
        }
    }

    public bool CheckPreSetList()
    {
        bool check = true;
        foreach (int i in preSetList)

        {
            if (i < 0)
            {
                check = false;
            }
        }

        if (!check)
        {
            ShowErrorMessage();
        }
        else
        {
            string listString = preSetList[0] + "," + preSetList[1] + "," + preSetList[2] + "," + preSetList[3];
            PlayManage.Instance.SkillPreSet = listString;
            PlayManage.Instance.SaveData();
        }
        return check;
    }

    public override bool AllowBackToLobby()
    {
        return CheckPreSetList();
    }

    public void ShowErrorMessage()
    {
        errorMessageWindow.gameObject.SetActive(true);
        StartCoroutine(errorMessageWindow.ShowMessage("ErrorMessageTest"));
    }


}
