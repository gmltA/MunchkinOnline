using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Longpool;
using Munchkin_Online.Models;
using Munchkin_Online.Core.Game.Cards;

namespace Munchkin_Online.Core.Game
{
    public class GameDirector
    {
        const string ACTION_DONE = "OK";
        const string ACTION_ERROR = "ERROR";

        Match match;

        public GameDirector(Match m)
        {
            match = m;
        }

        public string ProcessAction(ActionInfo info)
        {
            string result;
            if (match.BoardState.CurrentPlayerId != CurrentUser.Instance.Current.Id)
                result = ACTION_ERROR;

            Player player = match.BoardState.Players.First(x => x.UserId == CurrentUser.Instance.Current.Id);
            if (info.SourceEntry == ActionEntryType.Deck)
            {
                Card card = info.Source.GetRandomCard();

                info.Source = match.BoardState.DoorDeck;
                info.Source.RemoveCard(card);

                info.Target = info.Invoker;
                info.Target.AddCard(card);

                Longpool.Longpool.Instance.PushMessageToUser(player.UserId, new BattleMessage(player.UserId, card, info));
            }
            /*if (result != ACTION_ERROR)
            {
                foreach (var p in match.BoardState.Players)
                    Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new BattleMessage(player.UserId, card, info.TargetType));
            }*/
            return ACTION_DONE;
        }

        public string SetClass(Player player, Card card)
        {
            Class c;
            try
            {
                 c = (Class)Enum.Parse(typeof(Class),card.Name);
            }
            catch
            {
                return ACTION_ERROR;
            }
            if(player.Class == c)
                return ACTION_ERROR;
            player.Class = c;
            player.Board.Add(card);
            return ACTION_DONE;
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