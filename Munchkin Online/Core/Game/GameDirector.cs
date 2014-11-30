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
            // todo: handle action result
            string result;
            if (match.BoardState.CurrentPlayerId != CurrentUser.Instance.Current.Id)
                result = ACTION_ERROR;

            Player player = match.BoardState.Players.First(x => x.UserId == CurrentUser.Instance.Current.Id);
            Card card = null;
            if (info.TargetEntry == ActionEntryType.Hand && info.SourceEntry == ActionEntryType.Deck)
            {
                if (info.SourceParam == 0)
                    info.Source = match.BoardState.DoorDeck;
                else
                    info.Source = match.BoardState.TreasureDeck;

                card = info.Source.GetRandomCard();

                info.Target = info.Invoker.Hand;
            }
            if (info.SourceEntry == ActionEntryType.Hand && info.TargetEntry == ActionEntryType.Field)
            {
                info.Target = match.BoardState.Field;
                info.Source = info.Invoker.Hand;
                card = info.Source.GetCardById(info.CardId);
            }
            if (info.TargetEntry == ActionEntryType.Hand && info.SourceEntry == ActionEntryType.Slot)
            {
                info.Target = info.Invoker.Hand;
                info.Source = info.Invoker.Board;
                card = info.Source.GetCardById(info.CardId);
            }
            if (info.SourceEntry == ActionEntryType.Hand && info.TargetEntry == ActionEntryType.Slot)
            {
                info.Target = info.Invoker.Board;
                info.Source = info.Invoker.Hand;

                card = info.Source.GetCardById(info.CardId);

                if (!info.Invoker.TryEquip(card))
                    return ACTION_ERROR;
            }

            info.Source.RemoveCard(card);
            info.Target.AddCard(card);
            result = ACTION_DONE;

            if (result != ACTION_ERROR)
            {
                foreach (var p in match.BoardState.Players)
                    if (p.UserId != player.UserId)
                        Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new BattleMessage(player.UserId, card, info));
            }
            return ACTION_DONE;
        }
    }
}