using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase;
public class EmailLoginScript : MonoBehaviour {

    public InputField email;
    public InputField password;
    public Button button;
    FirebaseAuth auth;
    FirebaseDatabase database;
    DatabaseReference dbReference;
    // Use this for initialization
    void Start () {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
        dbReference = FirebaseDatabase.DefaultInstance.RootReference.Child("User");
        
        this.auth = FirebaseAuth.DefaultInstance;
        /*
        email.text = "asd3f@naver.com";
        password.text = "qwerQWER1234";
	    */
        Debug.Log("start");
        testDatabase();

    }
	public void signInAny() {
        Debug.Log("any");
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if(task.IsCompleted && !task.IsCanceled && !task.IsFaulted) {
                // User is now signed in.
                //FirebaseUser newUser = task.Result;
                //Debug.Log(auth.CurrentUser.UserId);
                //testDatabase();
                
                Debug.Log("db");
            }
            else {
                Debug.Log("Fail");
            }
        });

    }
    public void signInWithEmail() {

        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if(task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            else if(task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            else if(task.IsCompleted) {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
            }
        });
    }

    public void joinInWithEmail() {

        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
            if(task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if(task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            else if(task.IsCompleted) {
                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
            }
        });
    }
    void testDatabase() {
        dbReference.Push();
        Debug.Log("dbdb");
    }

    public void signOut() {
        FirebaseAuth.DefaultInstance.SignOut();
        

    }
}
