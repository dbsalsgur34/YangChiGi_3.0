using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2DObject : MonoBehaviour {

    private float Iconrotation;
    [Range(0, 360)]
    public int rotateAnglePerFrame;

    private void OnEnable()
    {
        Iconrotation = rotateAnglePerFrame * Time.deltaTime;
    }

    private void TargetIconRotate()
    {
        this.transform.localRotation *= Quaternion.Euler(0,0,Iconrotation);
    }
	
	// Update is called once per frame
	void Update () {
        TargetIconRotate();
	}
}
