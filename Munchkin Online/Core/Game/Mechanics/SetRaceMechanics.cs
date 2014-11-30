using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public class SetRaceMechanic : IMechanic
    {
        public string Execute(BoardState state, Player initiator, ITarget target)
        {
            return SetRace(initiator, target as Card);
        }

        public string SetRace(Player player, Card card)
        {
            Race c;
            try
            {
                c = (Race)Enum.Parse(typeof(Race), card.Name);
            }
            catch
            {
                return ACTION_ERROR;
            }
            if (player.Race == c)
                return ACTION_ERROR;
            player.Race = c;
            player.Board.Add(card);
            return ACTION_DONE;
        }
    }
}