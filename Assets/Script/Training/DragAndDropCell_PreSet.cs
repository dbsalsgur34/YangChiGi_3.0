using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropCell_PreSet : DragAndDropCell_Training {

    private int preSetCellNumber;

    public void SetSkillSetPanel(int cellNumber)
    {
        this.preSetCellNumber = cellNumber;
    }

    public override int? GetCellNumber()
    {
        return this.preSetCellNumber;
    }
}
