using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class NetworkMessageReceiver : MonoBehaviour {
    DatabaseReference logReference;
    public List<string> LogList;
    NetworkMessageReceiver()
    {

        logReference = FirebaseDatabase.DefaultInstance.GetReference("GameSessionComments");

        logReference.ChildAdded += HandleChildAdded;
    }


    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        // Do something with the data in args.Snapshot
    }

   
}
