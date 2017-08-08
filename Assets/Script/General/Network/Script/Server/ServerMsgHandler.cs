using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ServerSide
{
    public class ServerMsgHandler : MsgHandler {

        protected override void HandleMsg(string networkMessage)
        {
            string[] splitMsg = networkMessage.Split('/');
            switch (splitMsg[1])
            {
                case "Matching": ClientManager.Send(int.Parse(splitMsg[0]), "Matched");
                    break;
                default:
                    break;
            }
           
        }
    }

}
