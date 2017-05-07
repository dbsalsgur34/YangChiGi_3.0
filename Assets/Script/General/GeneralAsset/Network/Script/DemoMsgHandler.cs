using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMsgHandler : MsgHandler {
	#region implemented abstract members of MsgHandler

	protected override void HandleMsg (string networkMessage){
		Debug.Log ("MSG: " + networkMessage);
	}

	#endregion



}
