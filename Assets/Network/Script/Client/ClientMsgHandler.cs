using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientSide
{
    public class ClientMsgHandler : MsgHandler
    {
        public override void HandleMsg(string networkMessage)
        {
            string[] splitMsg = networkMessage.Split('/');
            switch (splitMsg[0])
            {
                case "PlayerNum":
                    KingGodClient.Instance.Playernum = (int.Parse(splitMsg[1]));
                    break;
                case "Seed":
                    KingGodClient.Instance.Seed = (int.Parse(splitMsg[1]));
                    StartCoroutine(PlayManage.Instance.LoadScene("YangChigi3.0"));
                    break;
                case "Start":
                    StartCoroutine(GameManager.GMInstance.ReadyScreen());
                    break;
                case "Shepherd":
                    GameManager.GMInstance.GetMessage(splitMsg[0],splitMsg[1]);
                    break;
                case "DequeComplete":
                    Destroy(KingGodClient.Instance.gameObject);
                    break;
                case "Skill":
                    GameManager.GMInstance.GetMessage(splitMsg[0], splitMsg[1]);
                    break;
                case "Out":
                    GameManager.GMInstance.GetMessage(splitMsg[0], splitMsg[1]);
                    break;
                case "GameEnd":
                    Debug.Log("Get");
                    StartCoroutine(PlayManage.Instance.LoadScene("Result"));
                    break;
                default:
                    break;
            }
        }
    }

    /*
     * 메세지 종류
     * 1) Seed/(int) : 양을 생성하는 랜덤 시드
     * 2) PlayerNum/(2 or 1) : 플레이어 넘버
     * 3) Start : 게임을 시작.
     * 4) Disconnect : 게임 종료.
     * 5) Shepherd_S/(PlayerNum, state, frame) : 양치기의 상태
     * 6) Skill_S/(PlayerNum, skillindex, x,y,z, frame)
     * 7) DequeComplete : Cancle 메세지를 날린 사람이 Get. 연결 대기를 취소한다.
     */
}
