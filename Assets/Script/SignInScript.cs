using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class SignInScript : MonoBehaviour {
    FirebaseAuth auth;
    FirebaseDatabase db;
    DatabaseReference dbReference;

    public InputField email;
    public InputField password;
    //public Button signInButton;
    //public Button newAccountButton;

	// Use this for initialization
	void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance;
        dbReference = db.RootReference;
        email.text = "asdf@naver.com";
        password.text = "asdfasdf";
	}
	
    public void SingInWithEmali() {
        if(auth.CurrentUser == null) {
            Debug.Log("Try to signIn");
            auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
                if(task.IsCanceled) {
                    Debug.Log(task.Exception);
                }else if(task.IsFaulted) {
                    Debug.Log(task.Exception);
                }else if(task.IsCompleted) {
                    Debug.Log(task.Result.UserId + "SignIn");
                }else {
                    Debug.Log("??");
                }
            });
        } else {

            Debug.Log("SignOut");
            auth.SignOut();
        }
    }

    public void CreateNewAccount() {
        Debug.Log("Try to create");
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if(task.IsCanceled) {
                Debug.Log(task.Exception);
            }
            else if(task.IsFaulted) {
                Debug.Log(task.Exception);
            }
            else if(task.IsCompleted) {
                Debug.Log(task.Result.UserId + "created");
            }
            else {
                Debug.Log("??");
            }
        });
    }
}
