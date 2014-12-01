using Munchkin_Online.Core.Matchmaking;
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
        public object Data { get; set; }

        public AsyncMessage(MessageType t)
        {
            Type = t;
        }

        public AsyncMessage(MessageType t, object Data)
        {
            Type = t;
            this.Data = Data;
        }

        public AsyncMessage(MatchInvite invite)
        {
            Type = MessageType.Invite;
            Data = invite;
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
        Notification,
        NoConfirm,
        QueueStatus,
        StopPooling,
        LobbyUpdate,
        GameStarted,
        InitialGameState,
        BattleMessage,
        PhaseChanged,
        DiceResult,
        EndOfTheBattle
    }
}