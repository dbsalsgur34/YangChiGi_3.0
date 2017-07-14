using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerBase : MonoBehaviour {

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    public virtual bool AllowBackToLobby()
    {
        return true;
    }
}
