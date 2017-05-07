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
    public bool IsItemCanDrag;

    public LayerMask LM;

    public float time;
    public Text timetext;

    private int num;
    private bool IsSkillNeedGuideLine;
    private GameManager GM;

    private void Start()
    {
        GM = GameManager.GMInstance;
        GM.GetSkillManager().SetSkillPanelQueue(this.gameObject.GetComponent<DragAndDropItem>());
        timetext = GetComponentInChildren<Text>();
        timetext.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsItemCanDrag)
        {
            sourceCell = GetComponentInParent<DragAndDropCell>();                       // Remember source cell
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

            GM.GetMainCamera().SetIsSkillCutScene(true);

            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this);                                             // Notify all about item drag start
            }
        }
    }

    public void SetInstance(int number, bool GuideLineNeed)
    {
        this.num = number;
        this.IsSkillNeedGuideLine = GuideLineNeed;
    }

    public void OnDrag(PointerEventData data)
    {
        if (IsItemCanDrag)
        {
            if (icon != null)
            {
                icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor
            }
            CheckIsIcononthePlanet();
        }
    }

    private void CheckIsIcononthePlanet()
    {
        RaycastHit hit = new RaycastHit();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray.origin, ray.direction,out hit,Mathf.Infinity,LM);

        if (hit.transform != null)
        {
            RazorActiveOption(true,true);
            GM.SetIsSkillOnthePlanaet(true);
            GM.hitVector = hit.point;
            GM.SetHitMarkerParentActive(true);
        }
        else
        {
            GM.SetHitMarkerParentActive(false);
            GM.SetIsSkillOnthePlanaet(true);
            RazorActiveOption(false,false);
        }
    }

    void RazorActiveOption(bool RCactive,bool CenterRCactive)
    {
        if (this.IsSkillNeedGuideLine)
        {
            GM.SetRazorActive(RCactive, CenterRCactive);
        }
        else
        {
            GM.SetRazorActive(false, CenterRCactive);
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

        GM.GetMainCamera().SetIsSkillCutScene(false);
        RazorActiveOption(false,false);
        GM.SetHitMarkerParentActive(false);
        if (GM.IsSkillCanUse())
        {
            KingGodClient.Instance.GetNetworkMessageSender().SendSkillVectorToServer(KingGodClient.Instance.Playernum,num,GM.hitVector,GameUIManager.GUIMInstance.GetTimePass());
            GM.SetIsSkillOnthePlanaet(false);
            IsItemCanDrag = false;
            GM.GetSkillManager().SetSkillPanelQueue(this.gameObject.GetComponent<DragAndDropItem>());
        }
        else { return; }
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

    public IEnumerator SkillDelay(float delay, Sprite WaitIcon, Sprite SkillIcon)
    {
        time = delay;
        this.gameObject.GetComponent<Image>().sprite = WaitIcon;
        timetext.gameObject.SetActive(true);
        yield return new WaitUntil(() => (time < 0));
        IsItemCanDrag = true;
        this.gameObject.GetComponent<Image>().sprite = SkillIcon;
        timetext.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timetext.text = time.ToString("N0");
        }
    }
}
