using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Every item's cell must contain this script
/// </summary>
[RequireComponent(typeof(Image))]
public class DragAndDropCell_Training : MonoBehaviour, IDropHandler
{
    public enum CellType
    {
        Swap,                                                               // Items will be swapped between cells
        DropOnly,                                                           // Item will be dropped into cell
        DragOnly,                                                           // Item will be dragged from this cell
        UnlimitedSource                                                     // Item will be cloned and dragged from this cell
    }
    public CellType cellType = CellType.Swap;                               // Special type of this cell

    public Color empty = new Vector4(1, 1, 1, 0.4f);                                   // Sprite color for empty cell
    public Color full = new Vector4(1, 1, 1, 0.4f);                                     // Sprite color for filled cell

    void OnEnable()
    {
        DragAndDropItem_Training.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem_Training.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
    }

    void OnDisable()
    {
        DragAndDropItem_Training.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem_Training.OnItemDragEndEvent -= OnAnyItemDragEnd;
    }

    private void OnAnyItemDragStart(DragAndDropItem_Training item)
    {
        DragAndDropItem_Training myItem = GetComponentInChildren<DragAndDropItem_Training>(); // Get item from current cell
        if (myItem != null)
        {
            myItem.MakeRaycast(false);                                      // Disable item's raycast for correct drop handling
            if (myItem == item)                                             // If item dragged from this cell
            {
                // Check cell's type
                switch (cellType)
                {
                    case CellType.DropOnly:
                        DragAndDropItem_Training.icon.SetActive(false);              // Item will not be dropped
                        break;
                    case CellType.UnlimitedSource:
                        // Nothing to do
                        break;
                    default:
                        item.MakeVisible(false);                            // Hide item in cell till dragging
                        break;
                }
            }
        }
    }

    private void OnAnyItemDragEnd(DragAndDropItem_Training item)
    {
        DragAndDropItem_Training myItem = GetComponentInChildren<DragAndDropItem_Training>(); // Get item from current cell
        if (myItem != null)
        {
            myItem.MakeRaycast(true);                                       // Enable item's raycast
        }

    }

    public void OnDrop(PointerEventData data)
    {
        if (DragAndDropItem_Training.icon != null)
        {
            if (DragAndDropItem_Training.icon.activeSelf == true)                    // If icon inactive do not need to drop item in cell
            {
                DragAndDropItem_Training item = DragAndDropItem_Training.draggedItem;
                DragAndDropCell_Training sourceCell = DragAndDropItem_Training.sourceCell;
                
                if ((item != null) && (sourceCell != this))
                {
                    switch (sourceCell.cellType)                            // Check source cell's type
                    {
                        case CellType.UnlimitedSource:
                            string itemName = item.name;
                            int itemnum = item.IndexNum;
                            item = Instantiate(item);                       // Clone item from source cell
                            item.name = itemName;
                            item.IndexNum = itemnum;
                            break;
                        default:
                            // Nothing to do
                            break;
                    }
                    switch (cellType)                                       // Check this cell's type
                    {
                        case CellType.Swap:
                            DragAndDropItem_Training currentItem = GetComponentInChildren<DragAndDropItem_Training>();
                            switch (sourceCell.cellType)
                            {
                                case CellType.Swap:
                                    SwapItems(sourceCell, this);            // Swap items between cells
                                    break;
                                default:
                                    PlaceItem(item.gameObject);             // Place dropped item in this cell
                                    break;
                            }
                            break;
                        case CellType.DropOnly:
                            PlaceItem(item.gameObject);                     // Place dropped item in this cell
                            break;
                        default:
                            //nothing to do.
                            break;
                    }
                }

                if (item.GetComponentInParent<DragAndDropCell_Training>() == null)   // If item have no cell after drop
                {
                    Destroy(item.gameObject);                               // Destroy it
                }

                TrainingManager.TMInstance.DragEndAction(sourceCell.GetCellNumber(),this.GetCellNumber(),item.IndexNum);
            }
        }     
    }

    public void RemoveItem()
    {
        foreach (DragAndDropItem_Training item in GetComponentsInChildren<DragAndDropItem_Training>())
        {
            Destroy(item.gameObject);
        }
        this.transform.DetachChildren();
       
    }

    public void PlaceItem(GameObject itemObj)
    {
        RemoveItem();                                                       // Remove current item from this cell
        if (itemObj != null)
        {
            itemObj.transform.SetParent(transform, false);
            itemObj.transform.localPosition = Vector3.zero;
            DragAndDropItem_Training item = itemObj.GetComponent<DragAndDropItem_Training>();
            if (item != null)
            {
                item.MakeRaycast(true);
            }
        }
    }

    public DragAndDropItem_Training GetItem()
    {
        if (this.transform.childCount.Equals(0))
            return null;
        else
        {
            return GetComponentInChildren<DragAndDropItem_Training>();
        }
    }

    public void SwapItems(DragAndDropCell_Training firstCell, DragAndDropCell_Training secondCell)
    {
        if ((firstCell != null) && (secondCell != null))
        {
            DragAndDropItem_Training firstItem = firstCell.GetItem();                // Get item from first cell
            DragAndDropItem_Training secondItem = secondCell.GetItem();              // Get item from second cell
            if (firstItem != null)
            {
                // Place first item into second cell
                firstItem.transform.SetParent(secondCell.transform, false);
                firstItem.transform.localPosition = Vector3.zero;
            }
            if (secondItem != null)
            {
                // Place second item into first cell
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
            }
        }
    }

    public virtual int? GetCellNumber()
    {
        return null;
    }

}