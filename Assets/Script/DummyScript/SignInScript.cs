using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class SignInScript : MonoBehaviour {
    FirebaseAuth auth;
    FirebaseDatabase db;

    public InputField email;
    public InputField password;

	// Use this for initialization
	void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");

        auth = FirebaseAuth.DefaultInstance;
	}
    
}
