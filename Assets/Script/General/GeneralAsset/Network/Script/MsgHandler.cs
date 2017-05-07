using UnityEngine;
using System.Collections;

public abstract class MsgHandler : MonoBehaviour {
	protected abstract void HandleMsg(string networkMessage);

    public void SetHandleMsg(string networkMessage)
    {
        HandleMsg(networkMessage);
    }
}
