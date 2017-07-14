using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class GameControlManager : GameManagerBase
{
    public enum TouchState
    {
        NORMALSTATE,
        SKILLDRAGSTATE
    }

    private RazorControl RC;
    private RazorControl CenterRC;

    private SpriteRenderer hitMarker;
    private GameObject hitMarkerParent;

    private Transform planetCenterPosition;
    private Transform HQPosition;
    public LayerMask hitCheckLayerMask;
    private TouchState touchState;
    private CameraControl mainCamera;
    private int RazorAdjustNum;

    protected override void Start()
    {
        base.Start();
        ManagerHandler.Instance.SetManager(this);
     }

    private void OnEnable()
    {
        ManagerHandler.Instance.SetManager(this);
        DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
        DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
    }

    protected override void InitManager()
    {
        base.InitManager();
        RC = GameObject.FindGameObjectWithTag("Razor").GetComponent<RazorControl>();
        CenterRC = GameObject.FindGameObjectWithTag("CenterRazor").GetComponent<RazorControl>();

        //스킬 이펙트 초기화
        hitMarkerInit();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControl>();
        touchState = TouchState.NORMALSTATE;
        SetPivotTransform();
        SetRazorAdjust();
        hitCheckLayerMask.value |= 1 << LayerMask.NameToLayer("Background"); // hitCheckLayerMask.value = hiCheckLayerMask.value | LayerMask.NameToLayer("Background")와 같음.

    }

    void OnDisable()
    {
        DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
        DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
    }

    private void SetPivotTransform()
    {
        planetCenterPosition = ManagerHandler.Instance.GameManager().GetPlanetTransform();
        HQPosition = ManagerHandler.Instance.GameManager().GetHQTransform();
    }

    private void SetRazorAdjust()
    {
        if (KingGodClient.Instance.playerNum.Equals(1))
        {
            RazorAdjustNum = 1;
        }
        else
        {
            RazorAdjustNum = -1;
        }
    }

    public void ChangeTouchState()
    {
        if (touchState.Equals(TouchState.NORMALSTATE))
        {
            touchState = TouchState.SKILLDRAGSTATE;
            mainCamera.SetIsSkillCutScene(true);
        }
        else
        {
            touchState = TouchState.NORMALSTATE;
            mainCamera.SetIsSkillCutScene(false);
        }
    }

    public TouchState GetTouchState()
    {
        return this.touchState;
    }

    private void hitMarkerInit()
    {
        hitMarker = GameObject.FindGameObjectWithTag("HitMarker").GetComponent<SpriteRenderer>();
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        hitMarkerParent = new GameObject("hitMarkerParent");                //create a new gameObject
        hitMarker.sprite = Resources.Load<Sprite>("Image/Resource/UI/select");
        hitMarker.gameObject.transform.parent = hitMarkerParent.transform;                    //make this camera a child of the new gameObject
        hitMarkerParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
        hitMarkerParent.SetActive(false);
    }

    private void ShowHitMarker(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.FromToRotation(hitMarkerParent.transform.up, target) * hitMarkerParent.transform.rotation;
        hitMarkerParent.transform.rotation = targetRotation;
    }

    private Vector3 SkillDragAction()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, hitCheckLayerMask);

        if (hit.transform != null)
        {
            SetHitMarkerParentActive(true);
            ShowHitMarker(hit.point);
            DrawRazor(hit.point);
            DrawCenterRazor(26);
        }
        else
        {
            SetHitMarkerParentActive(false);
            ShowHitMarker(Vector3.zero);
            DrawRazor(Vector3.zero);
            DrawCenterRazor(0);
        }
        return hit.point;
    }

    private void DrawRazor(Vector3 targetPoint)
    {
        RC.DrawCircle(planetCenterPosition.position,HQPosition.position,targetPoint,RazorAdjustNum);   
    }

    private void DrawCenterRazor(float radius)
    {
        CenterRC.DrawCenterCircle(planetCenterPosition.position, radius);
    }

    public void SetHitMarkerParentActive(bool state)
    {
        this.hitMarkerParent.SetActive(state);
    }

    public void SetRazorActive(bool RCactive, bool CenterRCactive)
    {
        RC.gameObject.SetActive(RCactive);
        CenterRC.gameObject.SetActive(CenterRCactive);
    }

    private void OnAnyItemDragStart(DragAndDropItem item)
    {
        ChangeTouchState();
        if (ManagerHandler.Instance.SkillManager().GetSkillDataBase().GetIsGuideLineNeed(item.GetSkillNumber()))
        {
            SetRazorActive(true, false);
        }
        else
        {
            SetRazorActive(false, false);
        }
    }

    private void OnAnyItemDragEnd(DragAndDropItem item)
    {
        ChangeTouchState();
        //Drag끝났을시 서버와 통신.
        Vector3 hitPosition = SkillDragAction();
        if (hitPosition != null)
        {
            ManagerHandler.Instance.NetworkManager().SendSkillToServer(item.GetSkillNumber(), hitPosition);
            ManagerHandler.Instance.SkillManager().SetSkillPanelQueue(item);
        }
        SetHitMarkerParentActive(false);
        SetRazorActive(false, false);
        
    }

    private void FixedUpdate()
    {
        if (this.touchState.Equals(TouchState.SKILLDRAGSTATE))
        {
            SkillDragAction();
        }
    }

}
