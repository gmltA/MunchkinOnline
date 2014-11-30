using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public class SetClassMechanic : IMechanic
    {
        public string Execute(BoardState state, Player initiator, ITarget target)
        {
            return SetClass(initiator, target as Card);
        }

        public string SetClass(Player player, Card card)
        {
            Class c;
            try
            {
                c = (Class)Enum.Parse(typeof(Class), card.Name);
            }
            catch
            {
                return ACTION_ERROR;
            }
            if (player.Class == c)
                return ACTION_ERROR;
            player.Class = c;
            player.Board.Add(card);
            return ACTION_DONE;
        }
    }
}