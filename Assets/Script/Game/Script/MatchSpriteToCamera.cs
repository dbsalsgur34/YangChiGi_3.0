using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSpriteToCamera : MonoBehaviour {

    public enum SpriteState
    {
        FlipEnable,
        FlipDisable,
        NoMove,
        VerticalFilpDisable,
        VerticalFlipEnable
    }

    public SpriteState spriteState;

    public int sortingOrderPreSet;
    private Transform cameraParent;
    private Transform Planet;
    private SpriteRenderer spriteRenderer;
    private Transform target;

    private float curRotation;
    private float preRotation;

    Vector3 negativeScale;
    Vector3 positiveScale;

    private void Start()
    {
        cameraParent = Camera.main.transform.parent;
        Planet = GameObject.Find("Planet").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingOrderPreSet = Mathf.Abs(sortingOrderPreSet);
        target = Planet;
        curRotation = 0f;
        preRotation = 0f;
        if (spriteState.Equals(SpriteState.FlipEnable) || spriteState.Equals(SpriteState.VerticalFlipEnable))
        {
            InvokeRepeating("FlipToTarget", 0f, 0.25f);
        }
        negativeScale = new Vector3(-1, 1, 1);
        positiveScale = new Vector3(1, 1, 1);
    }

    private float CheckAngleOfTwoVector(Vector3 Vector_one, Vector3 Vector_two)
    {
        return Vector3.Angle(Vector_one, Vector_two);
    }

    private float CalTowardRotationDegree(Vector3 a,Vector3 b)
    {
        Vector3 targetScreenPosition = Camera.main.WorldToViewportPoint(a);
        Vector3 thisScreenPosition = Camera.main.WorldToViewportPoint(b);
        Vector3 betweenVector = targetScreenPosition - thisScreenPosition;
        return 90 + Mathf.Atan2(betweenVector.y, betweenVector.x) * Mathf.Rad2Deg;
    }

    private void FlipToTarget()
    {
        curRotation = CalTowardRotationDegree(target.position, this.transform.position);
        if (curRotation - preRotation > 0)
        {
            StartCoroutine(FlipSmooth(this.transform.localScale,negativeScale));
        }
        else
        {
            StartCoroutine(FlipSmooth(this.transform.localScale, positiveScale));
        }
        preRotation = curRotation;
    }

    private IEnumerator FlipSmooth(Vector3 startVector, Vector3 targetVector)
    {
        for (float i = 0; i <= 1.1f; i += 0.05f)
        {
            this.transform.localScale = Vector3.Slerp(startVector, targetVector, i);
            yield return null;
        }
    }

    // Update is called once per frame
    private void Update ()
    {
        if (spriteState.Equals(SpriteState.FlipDisable) || spriteState.Equals(SpriteState.FlipEnable))
        {
            transform.rotation = cameraParent.rotation * Quaternion.Euler(0, 0, CalTowardRotationDegree(target.position, this.transform.position));
        }
        else if (spriteState.Equals(SpriteState.VerticalFilpDisable) || spriteState.Equals(SpriteState.VerticalFlipEnable))
        {
            transform.rotation = Quaternion.Euler(0, cameraParent.eulerAngles.y, 0);
        }
             

        if (CheckAngleOfTwoVector(this.transform.position - Planet.position, Camera.main.transform.position - Planet.position) > 65)
        {
            spriteRenderer.sortingOrder = -sortingOrderPreSet;
        }
        else
        {
            spriteRenderer.sortingOrder = sortingOrderPreSet;
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
