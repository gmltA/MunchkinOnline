using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public class RemoveAfterUseMechanic : IMechanic
    {
        public override string Execute(BoardState state, Player invoker, ITarget target)
        {
            return RemoveAfterUse(state, target as Card);
        }

        public string RemoveAfterUse(BoardState state, Card card)
        {
            state.Field.Cards.Remove(card);

            return ACTION_DONE;
        }
    }
}