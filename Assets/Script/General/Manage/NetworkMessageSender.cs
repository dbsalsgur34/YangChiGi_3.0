using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientSide;

public class NetworkMessageSender : MonoBehaviour
{
    public void SendSkillVectorToServer(int playerNum,int skillIndex,Vector3 targetVector, float messageSendTime)
    {
        Network_Client.Send("Skill/" + playerNum + "," + skillIndex + "," + targetVector.x + "," + targetVector.y + "," + targetVector.z + "," + messageSendTime);
    }

    public void SendPlayerEnemyPositionToServer(Vector3 playerPosition, Vector3 enemyPosition, float messageSendTime)
    {
        Network_Client.Send("Position/" + playerPosition + "," + enemyPosition + "," + messageSendTime);
    }

    public void SendReadyToServer(int playerNum)
    {
        Network_Client.Send("Ready/" + playerNum);
    }

    public void SendStartedToServer()
    {
        Network_Client.Send("started");
    }

    public void SendGameOverToServer(int playerNum, float messageSendTime)
    {
        Network_Client.Send("GameOver/" + playerNum + "," + messageSendTime);
    }

    public void SendPlayerStateToServer(int playerNum,int playerStateNum, float messageSendTime)
    {
        Network_Client.Send("Shepherd/" + playerNum + "," + playerStateNum + "," + messageSendTime);
    }

}
