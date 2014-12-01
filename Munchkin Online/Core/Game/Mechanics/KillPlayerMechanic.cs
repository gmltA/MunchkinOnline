using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public class KillPlayerMechanic : IMechanic
    {
        public override string Execute(BoardState state, Player invoker, ITarget target)
        {
            return Kill(invoker);
        }

        public string Kill(Player player)
        {
            foreach (var card in player.Hand.Cards)
                player.RemoveCard(card.Id);
            foreach (var card in player.Board.Cards)
                player.RemoveCard(card.Id);
            return ACTION_DONE;
        }
    }
}