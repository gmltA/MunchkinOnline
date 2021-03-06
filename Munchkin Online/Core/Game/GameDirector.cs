﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Longpool;
using Munchkin_Online.Models;
using Munchkin_Online.Core.Game.Cards;
using System.Web.Script.Serialization;
using Munchkin_Online.Core.Game.Mechanics;

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
            string result = ACTION_ERROR;
            object AdditionalData = null;
            // todo: handle action result
            if (match.BoardState.CurrentPlayerId != CurrentUser.Instance.Current.Id)
            {
                result = ACTION_ERROR;
                return MakeAnswer(result, AdditionalData);
            }

            Player player = match.BoardState.Players.First(x => x.UserId == CurrentUser.Instance.Current.Id);

            Card card = null;
            Object data = null;
            if (info.Type == ActionType.MoveCard)
            {
                if (ProcessMoveCardAction(info, out card))
                {
                    AdditionalData = card;
                    result = ACTION_DONE;
                }
            }
            else if (info.Type == ActionType.FinishTurn)
            {
                match.BoardState.StepFinished();
                data = match.BoardState.CurrentPlayerId;

                result = ACTION_DONE;
            }
            else if (info.Type == ActionType.TryEscape)
            {
                //todo: implement proper dice algorythm!
                var monsters = match.BoardState.Field.Cards.Where(c => c.Class == CardClass.Monster).ToList();
                if (monsters.Count > 0)
                {
                    ((List<IMechanic>)monsters[0].OnUseMechanics)[0].Execute(match.BoardState, info.Invoker, null);
                    data = new { diceResult = 1, escapeResult = false };
                    AdditionalData = data;
                    result = ACTION_DONE;
                    foreach (var p in match.BoardState.Players)
                    {
                        Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new AsyncMessage(MessageType.EndOfTheBattle));
                    }
                }
                else
                    result = ACTION_ERROR;
            }

            if (result != ACTION_ERROR)
            {
                foreach (var p in match.BoardState.Players)
                {
                    if (info.Type == ActionType.FinishTurn || p.UserId != player.UserId)
                        Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new BattleMessage(player.UserId, card, info, data));

                    Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new AsyncMessage(MessageType.PhaseChanged, match.BoardState.TurnStep));
                }
            }
            return MakeAnswer(result, AdditionalData);
        }

        bool ProcessMoveCardAction(ActionInfo info, out Card outCard)
        {
            Card card = null;
            if (info.TargetEntry == ActionEntryType.Field && info.SourceEntry == ActionEntryType.Deck)
            {
                if (info.SourceParam == 0)
                    info.Source = match.BoardState.DoorDeck;
                else
                    info.Source = match.BoardState.TreasureDeck;

                card = info.Source.GetRandomCard();

                info.Target = match.BoardState.Field;

                if (card.Class == CardClass.Monster)
                    match.BoardState.TurnStep = TurnStep.Battle;
                else
                    match.BoardState.TurnStep = TurnStep.Waiting;
            }
            if (info.TargetEntry == ActionEntryType.Hand && info.SourceEntry == ActionEntryType.Deck)
            {
                if (info.SourceParam == 0)
                    info.Source = match.BoardState.DoorDeck;
                else
                    info.Source = match.BoardState.TreasureDeck;

                card = info.Source.GetRandomCard();

                info.Target = info.Invoker.Hand;
                match.BoardState.TurnStep = TurnStep.Ending;
            }
            if (info.SourceEntry == ActionEntryType.Hand && info.TargetEntry == ActionEntryType.Field)
            {
                info.Target = match.BoardState.Field;
                info.Source = info.Invoker.Hand;
                card = info.Source.GetCardById(info.CardId);
                foreach (var m in card.OnUseMechanics)
                    m.Execute(match.BoardState, info.Invoker, card);
            }
            if (info.SourceEntry == ActionEntryType.Hand && info.TargetEntry == ActionEntryType.Slot)
            {
                info.Target = info.Invoker.Board;
                info.Source = info.Invoker.Hand;

                card = info.Source.GetCardById(info.CardId);

                if (!info.Invoker.TryEquip(card))
                {
                    outCard = null;
                    return false;
                }

                if (card.Class == CardClass.Race || card.Class == CardClass.Class)
                    card.OnUseMechanics.ElementAt(0).Execute(match.BoardState, info.Invoker, card);
            }

            if (card != null)
            {
                info.Source.RemoveCard(card);
                info.Target.AddCard(card);

                if (info.TargetEntry == ActionEntryType.Field)
                    foreach (var m in card.OnUseMechanics)
                        m.Execute(match.BoardState, info.Invoker, card);
            }

            outCard = card;
            return true;
        }

        public string MakeAnswer(string message, object data)
        {
            return new JavaScriptSerializer().Serialize(new { Message = message, Data = data });
        }
    }
}