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
        public ActionInfo Action { get; set; }


        public BattleMessage(Guid UserId, Card Card, ActionInfo Action, object data = null)
            : base(MessageType.BattleMessage, data)
        {
            this.UserId = UserId;
            this.Card = Card;
            this.Action = Action;
        }

    }
}