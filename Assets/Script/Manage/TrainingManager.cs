using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : ManagerBase
{
    public DragAndDropItem_Training[] SkillSetPanel;
    public DragAndDropItem_Training[] SkillListPanel;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        SkillListPanel = GameObject.FindGameObjectWithTag("SkillListPanel").GetComponentsInChildren<DragAndDropItem_Training>();
        
    }
}
