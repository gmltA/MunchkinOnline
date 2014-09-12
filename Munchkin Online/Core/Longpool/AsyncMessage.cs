using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Longpool
{
    public class AsyncMessage
    {
        public MessageType Type { get; set; }
        

        public AsyncMessage(MessageType t)
        {
            Type = t;
        }

        public override string ToString()
        {
            return "{ Type:" + Enum.GetName(typeof(MessageType), Type) + " }"; 
        }
    }

    public enum MessageType
    {
        OK,
        ERROR,
        NewLobby,
        FindedMatch,
        Invite
    }
}