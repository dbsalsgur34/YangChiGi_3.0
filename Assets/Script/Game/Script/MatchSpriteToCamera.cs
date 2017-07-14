using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSpriteToCamera : MonoBehaviour {

    public bool isRotateTowardCamera;
    public int sortingOrderPreSet;
    private Transform cameraParent;
    private Transform Planet;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        cameraParent = Camera.main.transform.parent;
        Planet = GameObject.Find("Planet").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingOrderPreSet = Mathf.Abs(sortingOrderPreSet);
    }

    private float CheckAngleOfTwoVector(Vector3 Vector_one, Vector3 Vector_two)
    {
        return Vector3.Angle(Vector_one, Vector_two);
    }

    // Update is called once per frame
    private void Update ()
    {
        if (isRotateTowardCamera)
        {
            transform.rotation = cameraParent.rotation;
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
}
