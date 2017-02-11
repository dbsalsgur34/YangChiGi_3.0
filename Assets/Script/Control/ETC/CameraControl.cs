using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraState
{
    FREE,
    LOCKONHQ,
    LOCKONPLAYER
}

public class CameraControl : MonoBehaviour
{
    // Attach this script to you camera
    // This script should likely be used in conjunction with a CameraLookAt script in unity's standard assets.
    // Script created by Alexander MacLeod - 14th December 2013

    public float tumbleSensitivity = 0.3f;        //NB. This value should be adjusted until a natural result is achieved. It affects the amount the camera will tumble around the object as a swipe takes place.
    public float LockOnSensitivity = 3f;
    public Transform pivotPoint;                //this should be the location the camera tumbles around
    public bool naturalMotion = true;            //this determines whether a left swipe will make the camera tumble clockwise or anticlockwise around the object
    public bool IsSkillCutScene;
    public int CameraNumber;
    public CameraState CS;

    private GameObject camParent;                //this will be the rotating parent to which the camera is attached. Rotating this object will have the effect of making the camera a specified location.
    private Vector2 oldInputPosition;            //records the position of the finger last update
    public GameObject Player;

    void Start()
    {
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        camParent = new GameObject("camParent");                //create a new gameObject
        camParent.transform.position = pivotPoint.position;        //place the new gameObject at pivotPoint location
        transform.parent = camParent.transform;                    //make this camera a child of the new gameObject
        camParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
        if (CameraNumber == 1)
        {
            Player = GameObject.Find("PlayerOne");
        }
        else if (CameraNumber == 2)
        {
            Player = GameObject.Find("PlayerTwo");
        }
        IsSkillCutScene = false;
        CS = CameraState.FREE;
    }

    void CameraAction()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            oldInputPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
                float xDif = Input.mousePosition.x - oldInputPosition.x;
                float yDif = Input.mousePosition.y - oldInputPosition.y;
                if (!naturalMotion) { xDif *= -1; yDif *= -1; }
                if (xDif != 0) { camParent.transform.Rotate(Vector3.up * xDif * tumbleSensitivity); }
                if (yDif != 0) { camParent.transform.Rotate(Vector3.right * yDif * tumbleSensitivity); }
                oldInputPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
                oldInputPosition = Input.mousePosition;
        }

#elif UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {

                    if (touch.phase == TouchPhase.Began && touch.fingerId == 0)
                    {
                        oldInputPosition = touch.position;
                    }
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        if (touch.fingerId == 0)
                        {
                            float xDif = touch.position.x - oldInputPosition.x;    //this calculates the horizontal distance between the current finger location and the location last frame.
                            float yDif = touch.position.y - oldInputPosition.y;
                            if (!naturalMotion) { xDif *= -1; yDif *= 1; }
                            if (xDif != 0) { camParent.transform.Rotate(Vector3.up * xDif * tumbleSensitivity); }
                            if (yDif != 0) { camParent.transform.Rotate(Vector3.right * yDif * tumbleSensitivity); }
                            oldInputPosition = touch.position;
                        }
                    }
                    if (touch.phase == TouchPhase.Ended && touch.fingerId == 0)
                    {
                        oldInputPosition = touch.position;
                    }
                
            }
        }
#endif

    }

    void CameraLockOnHQ()
    {
        camParent.transform.rotation = Quaternion.Slerp(camParent.transform.rotation,Quaternion.Euler(0,0,0),Time.deltaTime * LockOnSensitivity);
    }

    void CameraLockOnPlayer()
    {
        camParent.transform.rotation = Quaternion.Slerp(camParent.transform.rotation, Player.transform.rotation, LockOnSensitivity * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSkillCutScene)
        {
            if (CS == CameraState.FREE)
            {
                CameraAction();
            }
            else if (CS == CameraState.LOCKONHQ)
            {
                CameraLockOnHQ();
            }
            else if (CS == CameraState.LOCKONPLAYER)
            {
                CameraLockOnPlayer();
            }
        }
        else
        {
            return;
        }
    }
}

