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
    public void SetMatchReference(int MatchID)
    {
        MatchReference = DBR.Child("Match").Child(MatchID.ToString());
    }

    private void PushLog(string log)
    {
        MatchReference.Child("Log").Push().SetValueAsync(log);
    }

    private void PushMatch(string message)
    {
        //Network_Client.Send(message);
        MatchReference.Child("MatchInit").SetValueAsync(message).ContinueWith(task =>
        {

        });
    }
    
    public void SendSkillVector(int playerNum,int skillIndex,Vector3 targetVector, float messageSendTime)
    {
        targetMessage = "Skill/" + playerNum + "," + skillIndex + "," + targetVector.x + "," + targetVector.y + "," + targetVector.z + "," + (messageSendTime + timeDelay);
        PushLog(targetMessage);
    }

    public void SendPlayerEnemyPosition(Vector3 playerPosition, int playerNum, Vector3 enemyPosition, float messageSendTime)
    {
        targetMessage = "Position/" + playerNum + "," + playerPosition + "," + enemyPosition + "," + (messageSendTime + timeDelay);
        PushLog(targetMessage);
    }

    public void SendPlayerState(int playerNum,int playerStateNum, float messageSendTime)
    {
        if (GameTime.IsTimerStart())
        {
            targetMessage = ("Shepherd/" + playerNum + "," + playerStateNum + "," + (messageSendTime + timeDelay));
            PushLog(targetMessage);
        }
    }

    public void PushNewMatchSeed(int matchSeed)
    {
        targetMessage = "Seed/" + matchSeed;
        PushMatch(targetMessage);
    }

    public void SendGameOver(int playerNum, float messageSendTime)
    {
        targetMessage = "GameOver/" + playerNum + "," + (messageSendTime + timeDelay);
        PushMatch(targetMessage);
    }

    public void SendReady(int playerNum)
    {
        targetMessage = "Ready/" + playerNum;
        PushMatch(targetMessage);
    }

    public void SendStarted()
    {
        targetMessage = "Started";
        PushMatch(targetMessage);
    }

}
