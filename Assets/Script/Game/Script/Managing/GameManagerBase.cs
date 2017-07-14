using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : ManagerBase {

    private bool IsInit;
    protected override void Awake()
    {
        IsInit = false;
    }

    protected virtual void InitManager()
    {

    }

    public IEnumerator StartManagerInit()
    {
        if (IsInit.Equals(false))
        {
            InitManager();
            yield return null;
        }
        yield return IsInit = true;
    }
}
