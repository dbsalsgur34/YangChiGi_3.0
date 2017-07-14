using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour {

    protected Button B;

    // Use this for initialization
    protected virtual void Start ()
    {
	    B = this.gameObject.GetComponent<Button>();
    }

    protected void AddButtonClickEvent(UnityAction EventAction)
    {
        B.onClick.AddListener(EventAction);
    }

}
