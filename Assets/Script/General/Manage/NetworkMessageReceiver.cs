﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using ClientSide;

public class NetworkMessageReceiver
{
    DatabaseReference logReference;
	
    public List<string> logList;

    public NetworkMessageReceiver()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
		Debug.Log("NMR created");
	}


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}

		// Do something with the data in args.Snapshot
		
		string[] splitMsg = args.Snapshot.Value.ToString().Split('/');
		//Debug.Log(splitMsg[0] + splitMsg[1]);
		GameManager.GMInstance.GetMessage(splitMsg[0], splitMsg[1]);
	}

	public void SetLogReference(string matchID)
	{
		this.logReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Match").Child(matchID).Child("Log");
		this.logReference.ChildAdded += HandleChildAdded;
		logList = new List<string>();
		Debug.Log("SetLogReference");
	}
}
