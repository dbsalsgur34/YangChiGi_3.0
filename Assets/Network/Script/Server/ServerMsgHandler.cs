using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ServerSide
{
    public class ServerMsgHandler : MsgHandler {

        public override void HandleMsg(string networkMessage)
        {
            
            string[] splitMsg = networkMessage.Split('@','/');
            switch (splitMsg[1])
            {
                case "Cancle":
                    ClientManager.DeQueClient(int.Parse(splitMsg[0]));
                    break;
                case "Ready":
                    ClientManager.ready(int.Parse(splitMsg[0]));
                    break;
				case "GameOver":
					ClientManager.MatchEnd(int.Parse(splitMsg[0]));
					break;
                default:
                    ClientManager.BroadCastToMatch(int.Parse(splitMsg[0]),splitMsg[1]+"/"+splitMsg[2]);
                    break;
            }
           
        }
    }

}
