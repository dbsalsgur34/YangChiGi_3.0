using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : GameManagerBase
{
    private SkillDataBase SkillDB;
    private Queue<DragAndDropItem> SkillPanelQueue;
    public DragAndDropItem[] SkillPanelList;
    private List<int> preSetList;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
    }

    protected override void InitManager()
    {
        base.InitManager();
        //Awake에서 실행되던 것들
        SkillPanelList = new DragAndDropItem[4];
        SkillPanelQueue = new Queue<DragAndDropItem>();
        preSetList = new List<int>();

        //Start에서 실행되던 것들
        //스킬 관련 초기화.
        SkillDB = GameObject.FindGameObjectWithTag("SkillDataBase").GetComponent<SkillDataBase>();
        string[] tempList = PlayManage.Instance.SkillPreSet.Split(',');
        EnrollSkillPanelList();
        for (int i = 0; i < tempList.Length; i++)
        {
            preSetList.Add(int.Parse(tempList[i]));
        }
        StartCoroutine(InitSkillPanel());
        SkillPanelQueue = new Queue<DragAndDropItem>();
    }

    private IEnumerator InitSkillPanel()
    {
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < preSetList.Count; i++)
        {
            int index = preSetList[i];
            SkillPanelList[i].SetInstance(index,SkillDB.GetSkillCoolTime(index),SkillDB.GetSkillIcon(index));
            StartCoroutine(SkillPanelList[i].SkillDelay());
        }
    }

    public void UsingSkill(int SkillNumber, PlayerControlThree Owner, GameObject Target, Transform Pivot, Transform Pivotrotation,float angle, Vector3 skillVector)
    {
        GameObject ActivatedSkill = Instantiate(SkillDB.skillPrefab[SkillNumber]);
        SkillBase ActivatedSkillInit = ActivatedSkill.GetComponent<SkillBase>();
        ActivatedSkillInit.SetInstance(Owner, Target);
        ActivatedSkillInit.SetPivot(Pivot,Pivotrotation,angle,skillVector);
        SetSkillPanelSkillDelay();
    }

    public void SetSkillPanelQueue(DragAndDropItem item)
    {
        SkillPanelQueue.Enqueue(item);
    }

    public void SetSkillPanelList(int cellIndex, DragAndDropItem item)
    {
        SkillPanelList[cellIndex] = (item);
    }

    public void EnrollSkillPanelList()
    {
        DragAndDropItem[] temp = GameObject.FindObjectsOfType<DragAndDropItem>();

        for (int i = 0; i < temp.Length; i++)
        {
            SetSkillPanelList(temp[i].transform.parent.GetSiblingIndex(),temp[i]);
        }
    }

    private void SetSkillPanelSkillDelay()
    {
        StartCoroutine(SkillPanelQueue.Dequeue().SkillDelay());
    }

    public SkillDataBase GetSkillDataBase()
    {
        return this.SkillDB;
    }

    /*private void FixedUpdate()
    {
        if (GameTime.IsTimerStart())
        {
            if (SkillPanelQueue.Count > 0)
            {
                UseSkillPanelSkill();
            }
        }
    }*/
}
