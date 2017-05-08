using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour {

    public Vector3 fixPosition;

	// Use this for initialization
	void Start ()
    {
        this.transform.position = fixPosition;
	}
	
}
