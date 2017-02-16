using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ServerSide
{
    public class ServerMsgHandler : MsgHandler {

        public override void HandleMsg(string networkMessage)
        {
            
            string[] splitMsg = networkMessage.Split('@', '/');
            switch (splitMsg[1])
            {
                case "Cancle":
                    ClientManager.DeQueClient(int.Parse(splitMsg[0]));
                        
                default:
                    ClientManager.BroadCast(networkMessage);
                    break;
            }
           
        }
    }

}
