using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ClientSide;

public class DragAndDropItem_Training : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public DragAndDropItem_Training draggedItem;                             // Item that is dragged now
    static public GameObject icon;                                                  // Icon of dragged item
    static public DragAndDropCell_Training sourceCell;                              // From this cell dragged item is

    public delegate void DragEvent(DragAndDropItem_Training item);
    static public event DragEvent OnItemDragStartEvent;                             // Drag start event
    static public event DragEvent OnItemDragEndEvent;                               // Drag end event
    public bool IsItemCanDrag;

    public int indexnum;
    public int RequiredLevel;

    private void Start()
    {
        if (PlayManage.Instance.playerlevel < RequiredLevel)
        {
            IsItemCanDrag = false;
            this.gameObject.GetComponent<Image>().sprite = PlayManage.Instance.GetSkillDataBase().SetSkillIcon(indexnum,true);
            ShowRequiredLevel();
        }
        else
        {
            IsItemCanDrag = true;
            this.gameObject.GetComponent<Image>().sprite = PlayManage.Instance.GetSkillDataBase().SetSkillIcon(indexnum,false);
        }
    }

    void ShowRequiredLevel()
    {
        Text text = (Instantiate(Resources.Load("Prefab/LevelText"), this.gameObject.transform) as GameObject).GetComponent<Text>();
        text.text = "Lv." + RequiredLevel.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsItemCanDrag)
        {
            sourceCell = GetComponentInParent<DragAndDropCell_Training>();              // Remember source cell
            draggedItem = this;                                                         // Set as dragged item
            icon = new GameObject("Icon");                                              // Create object for item's icon
            Image image = icon.AddComponent<Image>();
            image.sprite = GetComponent<Image>().sprite;
            image.raycastTarget = false;                                                // Disable icon's raycast for correct drop handling
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            // Set icon's dimensions
            iconRect.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x,
                                                GetComponent<RectTransform>().sizeDelta.y);
            Canvas canvas = GetComponentInParent<Canvas>();                             // Get parent canvas
            if (canvas != null)
            {
                // Display on top of all GUI (in parent canvas)
                icon.transform.SetParent(canvas.transform, true);                       // Set canvas as parent
                icon.transform.SetAsLastSibling();                                      // Set as last child in canvas transform
            }

            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this);                                             // Notify all about item drag start
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (IsItemCanDrag)
        {
            if (icon != null)
            {
                icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (icon != null)
        {
            Destroy(icon);                                                          // Destroy icon on item drop
        }
        MakeVisible(true);                                                          // Make item visible in cell
        if (OnItemDragEndEvent != null)
        {
            OnItemDragEndEvent(this);                                               // Notify all cells about item drag end
        }

        if (eventData.pointerCurrentRaycast.gameObject.tag != "SkillPanel" && sourceCell.cellType == DragAndDropCell_Training.CellType.Swap)
        {
            sourceCell.RemoveItem();
        }

        draggedItem = null;
        icon = null;
        sourceCell = null;
    }

    public void MakeRaycast(bool condition)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = condition;
        }
    }

    public void MakeVisible(bool condition)
    {
        GetComponent<Image>().enabled = condition;
    }
}

