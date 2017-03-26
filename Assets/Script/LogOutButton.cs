using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogOutButton : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Button B = this.gameObject.GetComponent<Button>();
        B.onClick.AddListener(SignOut);
	}
	
	void SignOut()
    {
        PlayManage.Instance.Signout();
        Debug.Log("YES");
    }
}