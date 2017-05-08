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

    public struct DropDescriptor                                            // Struct with info about item's drop event
    {
        public DragAndDropCell_Training sourceCell;                                  // From this cell item was dragged
        public DragAndDropCell_Training destinationCell;                             // Into this cell item was dropped
        public DragAndDropItem_Training item;                                        // dropped item
        public bool IsItemDropSucess;
    }

    public Color empty = new Color();                                       // Sprite color for empty cell
    public Color full = new Color();                                        // Sprite color for filled cell

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

    void Start()
    {
        SetBackgroundState(GetComponentInChildren<DragAndDropItem_Training>() == null ? false : true);
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
                        SetBackgroundState(false);
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
            if (myItem == item)
            {
                SetBackgroundState(true);
            }
            myItem.MakeRaycast(true);                                       // Enable item's raycast
        }
        else
        {
            SetBackgroundState(false);
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
                DropDescriptor desc = new DropDescriptor();
                if ((item != null) && (sourceCell != this))
                {
                    switch (sourceCell.cellType)                            // Check source cell's type
                    {
                        case CellType.UnlimitedSource:
                            string itemName = item.name;
                            int itemnum = item.indexnum;
                            item = Instantiate(item);                       // Clone item from source cell
                            item.name = itemName;
                            item.indexnum = itemnum;
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
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    // Send message with DragAndDrop info to parents GameObjects
                                    StartCoroutine(NotifyOnDragEnd(desc));
                                    if (currentItem != null)
                                    {
                                        // Fill event descriptor
                                        desc.item = currentItem;
                                        desc.sourceCell = this;
                                        desc.destinationCell = sourceCell;
                                        // Send message with DragAndDrop info to parents GameObjects
                                        StartCoroutine(NotifyOnDragEnd(desc));
                                    }
                                    break;
                                default:
                                    PlaceItem(item.gameObject);             // Place dropped item in this cell
                                    // Fill event descriptor
                                    desc.item = item;
                                    desc.sourceCell = sourceCell;
                                    desc.destinationCell = this;
                                    // Send message with DragAndDrop info to parents GameObjects
                                    StartCoroutine(NotifyOnDragEnd(desc));
                                    break;
                            }
                            break;
                        case CellType.DropOnly:
                            PlaceItem(item.gameObject);                     // Place dropped item in this cell
                            // Fill event descriptor
                            desc.item = item;
                            desc.sourceCell = sourceCell;
                            desc.destinationCell = this;
                            // Send message with DragAndDrop info to parents GameObjects
                            StartCoroutine(NotifyOnDragEnd(desc));
                            break;
                        default:
                            // Nothing to do
                            break;
                    }
                }

                if (item.GetComponentInParent<DragAndDropCell_Training>() == null)   // If item have no cell after drop
                {
                    Destroy(item.gameObject);                               // Destroy it
                }
            }
        }     
    }



    private void SetBackgroundState(bool condition)
    {
        GetComponent<Image>().color = condition ? full : empty;
    }

    public void RemoveItem()
    {
        foreach (DragAndDropItem_Training item in GetComponentsInChildren<DragAndDropItem_Training>())
        {
            Destroy(item.gameObject);
        }
        SetBackgroundState(false);
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
            SetBackgroundState(true);
        }
    }

    public DragAndDropItem_Training GetItem()
    {
        return GetComponentInChildren<DragAndDropItem_Training>();
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
                secondCell.SetBackgroundState(true);
            }
            if (secondItem != null)
            {
                // Place second item into first cell
                secondItem.transform.SetParent(firstCell.transform, false);
                secondItem.transform.localPosition = Vector3.zero;
                firstCell.SetBackgroundState(true);
            }
        }
    }

    private IEnumerator NotifyOnDragEnd(DropDescriptor desc)
    {
        // Wait end of drag operation
        while (DragAndDropItem_Training.draggedItem != null)
        {
            yield return new WaitForEndOfFrame();
        }
        // Send message with DragAndDrop info to parents GameObjects
        gameObject.SendMessageUpwards("OnItemPlace", desc, SendMessageOptions.DontRequireReceiver);
    }
}