using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour {

    public RectTransform positiveMarkerPrefab;
    public RectTransform NegetiveMarketPrefab;
    public RectTransform Compassbar;
    public GameObject PlayerObject;
    public GameObject TargetObject;
    public 
    void ShowTargetObjectInCompassbar()
    {
        float angle;
        Vector3 PO = PlayerObject.transform.position + new Vector3(0, 1, 0);
        Vector3 TO = TargetObject.transform.position + new Vector3(0, 1, 0);
        Vector3 PTVector = TO - PO;
        angle = Vector3.Dot(PlayerObject.transform.right, PTVector);    //플레이어의 오른쪽 벡터를 기준으로 내적.
        positiveMarkerPrefab.localPosition = new Vector3(Mathf.Clamp(angle * 24,-300,300), 0, 0);
    }

	// Update is called once per frame
	void Update () {
        ShowTargetObjectInCompassbar();
	}
}
