using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public class EvadeMonstersMechanic : IMechanic
    {
        public override string Execute(BoardState state, Player invoker, ITarget target)
        {
            return EvadeMonsters(state, invoker);
        }

        public string EvadeMonsters(BoardState state, Player player)
        {
            state.Field.Cards.Clear();
            state.TurnStep = TurnStep.Ending;
            foreach (var p in state.Players)
                Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new Longpool.AsyncMessage(Longpool.MessageType.EndOfTheBattle));
            return ACTION_DONE;
        }
    }
}