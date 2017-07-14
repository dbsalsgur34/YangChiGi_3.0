using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using ClientSide;

public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    static public DragAndDropItem draggedItem;                                      // Item that is dragged now
    static public GameObject icon;                                                  // Icon of dragged item
    static public DragAndDropCell sourceCell;                                       // From this cell dragged item is

    public delegate void DragEvent(DragAndDropItem item);
    static public event DragEvent OnItemDragStartEvent;                             // Drag start event
    static public event DragEvent OnItemDragEndEvent;                               // Drag end event

    public struct ItemInstance
    {
        public bool IsItemCanDrag;
        public float coolTime;
        public int skillIndexNum;
        public Sprite waitIcon;
        public Sprite skillIcon;

        public ItemInstance(int skillIndexNum, float coolTime, Sprite skillIcon)
        {
            this.skillIndexNum = skillIndexNum;
            this.coolTime = coolTime;
            this.skillIcon = skillIcon;
            IsItemCanDrag = false;
            waitIcon = Resources.Load<Sprite>("Image/Resource/Button/Black/SkillIcon/stopwatch");
        }
    }

    private float time;
    private Text timetext;
    private Image itemImage;
    private ItemInstance itemInstance;

    private void Start()
    {
        timetext = GetComponentInChildren<Text>();
        timetext.gameObject.SetActive(false);
        //ManagerHandler.Instance.SkillManager().SetSkillPanelList(transform.parent.GetSiblingIndex(),this);
        itemImage = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemInstance.IsItemCanDrag)
        {
            sourceCell = GetComponentInParent<DragAndDropCell>();                       // Remember source cell
            draggedItem = this;                                                         // Set as dragged item
            icon = new GameObject("Icon");                                              // Create object for item's icon
            Image image = icon.AddComponent<Image>();
            image.sprite = itemImage.sprite;
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

    public void SetInstance(int indexNumber,float coolTime, Sprite skillImage)
    {
        this.itemInstance = new ItemInstance(indexNumber,coolTime,skillImage);
    }

    public int GetSkillNumber()
    {
        return this.itemInstance.skillIndexNum; 
    }

    public void OnDrag(PointerEventData data)
    {
        if (itemInstance.IsItemCanDrag)
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

    public IEnumerator SkillDelay()
    {
        time = itemInstance.coolTime;
        itemImage.sprite = itemInstance.waitIcon;
        timetext.gameObject.SetActive(true);
        yield return new WaitUntil(() => (time < 0));
        itemInstance.IsItemCanDrag = true;
        itemImage.sprite = itemInstance.skillIcon;
        timetext.gameObject.SetActive(false);
    }

    public void SetItemDragDisable()
    {
        this.itemInstance.IsItemCanDrag = false;
    }

    private void Update()
    {
        if (time > 0 && GameTime.IsTimerStart())
        {
            time -= Time.deltaTime;
            timetext.text = time.ToString("N0");
        }
    }
}
