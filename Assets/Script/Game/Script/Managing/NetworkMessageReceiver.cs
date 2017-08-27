using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using ClientSide;

public class NetworkMessageReceiver
{
    DatabaseReference matchReference;
    DatabaseReference logReference;

    public List<string> logList;

    public NetworkMessageReceiver()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
		Debug.Log("NMR created");
	}


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        Debug.Log("GotMessage");
        if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}

        // Do something with the data in args.Snapshot
        ManagerHandler.Instance.NetworkManager().SetMessageQueue(args.Snapshot.Value.ToString());
	}
    public void SetMatchReference(int matchID)
    {
        this.matchReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Match").Child(matchID.ToString());
        this.matchReference.ChildAdded += HandleChildAdded;
        logList = new List<string>();
        Debug.Log("SetLogReference");
    }
    public void SetLogReference(int matchID)
	{
		this.logReference = FirebaseDatabase.DefaultInstance.RootReference.Child("Match").Child(matchID.ToString()).Child("Log");
		this.logReference.ChildAdded += HandleChildAdded;
		logList = new List<string>();
		Debug.Log("SetLogReference");
	}
}
