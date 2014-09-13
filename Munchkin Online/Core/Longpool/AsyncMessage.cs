using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Munchkin_Online.Core.Longpool
{
    public class AsyncMessage
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }

        public AsyncMessage(MessageType t)
        {
            Type = t;
        }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this); 
        }
    }

    public enum MessageType
    {
        OK,
        ERROR,
        NewLobby,
        FindedMatch,
        Invite,
        Notification
    }
}