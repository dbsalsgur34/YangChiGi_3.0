using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using ClientSide;

public class NetworkMessageSender
{
    DatabaseReference DBR;

    DatabaseReference UserReference;
    DatabaseReference MatchReference;
    private string targetMessage;
    private float timeDelay = 0.5f;

    public NetworkMessageSender()
    {
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yang-chigi.firebaseio.com/");
		DBR = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void SetMatchReference(string MatchID)
    {
        MatchReference = DBR.Child("Match").Child(MatchID);
    }

    private void PushLogToDatabase(string log)
    {
        MatchReference.Child("Log").Push().SetValueAsync(log);
    }

    private void Send(string message)
    {
        Network_Client.Send(message);
       
    }
    
    public void SendSkillVectorToServer(int playerNum,int skillIndex,Vector3 targetVector, float messageSendTime)
    {
        targetMessage = "Skill/" + playerNum + "," + skillIndex + "," + targetVector.x + "," + targetVector.y + "," + targetVector.z + "," + (messageSendTime + timeDelay);
        PushLogToDatabase(targetMessage);
    }

    public void SendPlayerEnemyPositionToServer(Vector3 playerPosition, int playerNum, Vector3 enemyPosition, float messageSendTime)
    {
        targetMessage = "Position/" + playerNum + "," + playerPosition + "," + enemyPosition + "," + (messageSendTime + timeDelay);
        PushLogToDatabase(targetMessage);
    }

    public void SendReadyToServer(int playerNum)
    {
        targetMessage = "Ready/" + playerNum;
        Send(targetMessage);
    }

    public void SendStartedToServer()
    {
        targetMessage = "Started";
        Send(targetMessage);
    }

    public void SendGameOverToServer(int playerNum, float messageSendTime)
    {
        targetMessage = "GameOver/" + playerNum + "," + (messageSendTime + timeDelay);
        Send(targetMessage);
    }

    public void SendPlayerStateToServer(int playerNum,int playerStateNum, float messageSendTime)
    {
        targetMessage = ("Shepherd/" + playerNum + "," + playerStateNum + "," + (messageSendTime + timeDelay));
        PushLogToDatabase(targetMessage);
    }

}
