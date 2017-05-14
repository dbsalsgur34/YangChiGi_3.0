﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class NetworkMessageReceiver : MonoBehaviour {
    DatabaseReference logReference;
	
    public List<string> logList;

    public NetworkMessageReceiver()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
		
	}


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

		// Do something with the data in args.Snapshot
		logList.Add(args.Snapshot.GetValue(true).ToString());
		Debug.Log(logList.Count + " : " + logList[logList.Count-1]);
	}

	public void SetLogReference(string matchID)
	{
		this.logReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Match").Child(matchID).Child("Log");
		this.logReference.ChildAdded += HandleChildAdded;
		logList = new List<string>();
	}
}