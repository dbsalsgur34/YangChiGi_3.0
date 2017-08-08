using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
public class MatchFinderScript : MonoBehaviour {
    //랜덤이 Monobehaviour에서 스태틱으로 지정해놔서 저럴걸ㅇㅇ
    // Use this for initialization
    public Button playButton;
    
    private float randomValue;
    DatabaseReference matchQueRef;
	void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
        matchQueRef = FirebaseDatabase.DefaultInstance.RootReference.Child("matchingQue");
        randomValue = Random.value;    
        playButton.onClick.AddListener(RegistToMatchQue);
	}

    void RegistToMatchQue()
    {
        //DatabaseReference matchRef = 
        string temp = randomValue.ToString();
        matchQueRef.ChildAdded += HandleChildAdded;
        matchQueRef.Push().SetValueAsync(randomValue.ToString()).ContinueWith(task =>
        {
            

        });

    }
    void Waitting()
    {

    }

    void Finding()
    {

    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        Debug.Log(args.Snapshot.Value.ToString());
    }
}
