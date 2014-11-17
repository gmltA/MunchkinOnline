using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Longpool
{
    public class BattleMessage : AsyncMessage
    {
        public Guid UserId { get; set; }
        public Card Card { get; set; }
        public TargetType TargetType { get; set; }


        public BattleMessage(Guid UserId, Card Card, TargetType TargetType, object data = null )
            : base(MessageType.BattleMessage, data)
        {
            this.UserId = UserId;
            this.Card = Card;
            this.TargetType = TargetType;
        }

    }
}