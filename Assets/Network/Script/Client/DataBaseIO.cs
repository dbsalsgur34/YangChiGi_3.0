using System.Collections;
using System.Collections.Generic;

using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class DataBaseIO  {
	FirebaseDatabase rootDB;
	DatabaseReference rootRefer;
	DatabaseReference logRefer;

	public DataBaseIO()
	{
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
		rootDB = FirebaseDatabase.DefaultInstance;
		rootRefer = rootDB.RootReference;
		logRefer = rootRefer.Child("Match").Push().Child("Log");
	}

	public void push(string msg)
	{
		logRefer.Push().SetValueAsync(msg);
	}
}
