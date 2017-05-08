using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using ClientSide;

public class NetworkMessageSender : MonoBehaviour
{
    FirebaseDatabase DB;
    DatabaseReference DBR;

    DatabaseReference UserReference;
    DatabaseReference MatchReference;
    private string targetMessage;
    public NetworkMessageSender()
    {
        DB = FirebaseDatabase.DefaultInstance;
        DBR = DB.RootReference;
    }
    public void SetMatchReference(string MatchID)
    {
        MatchReference = DBR.Child("Match").Child(MatchID);
    }

    private void PushLogToDatabase(string log)
    {
        MatchReference.Child("Log").Push().SetValueAsync("log");
    }

    private void SendAndPush(string message)
    {
        Network_Client.Send(message);
        PushLogToDatabase(message);
    }
    
    public void SendSkillVectorToServer(int playerNum,int skillIndex,Vector3 targetVector, float messageSendTime)
    {
        targetMessage = "Skill/" + playerNum + "," + skillIndex + "," + targetVector.x + "," + targetVector.y + "," + targetVector.z + "," + messageSendTime;
        SendAndPush(targetMessage);
    }

    public void SendPlayerEnemyPositionToServer(Vector3 playerPosition, Vector3 enemyPosition, float messageSendTime)
    {
        targetMessage = "Position/" + playerPosition + "," + enemyPosition + "," + messageSendTime;
        SendAndPush(targetMessage);
    }

    public void SendReadyToServer(int playerNum)
    {
        targetMessage = "Ready/" + playerNum;
        SendAndPush(targetMessage);
    }

    public void SendStartedToServer()
    {
        targetMessage = "started";
        SendAndPush(targetMessage);
    }

    public void SendGameOverToServer(int playerNum, float messageSendTime)
    {
        targetMessage = "GameOver/" + playerNum + "," + messageSendTime;
        SendAndPush(targetMessage);
    }

    public void SendPlayerStateToServer(int playerNum,int playerStateNum, float messageSendTime)
    {
        targetMessage = ("Shepherd/" + playerNum + "," + playerStateNum + "," + messageSendTime);
        SendAndPush(targetMessage);
    }

}
