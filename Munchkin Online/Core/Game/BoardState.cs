﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game.Cards;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game
{
    public class BoardState
    {
        public const int PLAYERS_COUNT = 4;
        public const int DOORS_COUNT = 0;
        public const int TREASURES_COUNT = 0;

        public CardHolder Field { get; set; }

        public CardHolder DoorDeck { get; set; }
        public CardHolder DoorTrash { get; set; }

        public CardHolder TreasureDeck { get; set; }
        public CardHolder TreasureTrash { get; set; }

        public Guid CurrentPlayerId { get; set; }
        public List<Player> Players { get; set; }

        public TurnStep TurnStep { get; set; }
        public Battle Battle { get; set; }

        public BoardState(List<Player> players)
        {
            Players = players;

            Field = new CardHolder();

            DoorDeck = new CardHolder();
            DoorTrash = new CardHolder();

            TreasureDeck = new CardHolder();
            TreasureTrash = new CardHolder();
            Random r = new Random();

            int k;
            List<Card> cards = CardsContainer.GetCards();
            for (int i = 1; i <= DOORS_COUNT; i++ )
                foreach (var p in Players)
                {
                    
                    try
                    {

                        k = r.Next(cards.Where(x => x.Type == CardType.Dungeon).Count() - 1);
                    }
                    catch
                    {
                        k = 0;
                    }
                    Card c = cards.Where(x => x.Type == CardType.Dungeon).ElementAt(k);
                    p.Hand.Cards.Add(c);
                    cards.Remove(c);
                }
            for (int i = 1; i <= TREASURES_COUNT; i++)
                foreach (var p in Players)
                {
                    try
                    {

                        k = r.Next(cards.Where(x => x.Type == CardType.Treasure).Count() - 1);
                    }
                    catch
                    {
                        k = 0;
                    }
                    Card c = cards.Where(x => x.Type == CardType.Treasure).ElementAt(k);
                    p.Hand.Cards.Add(c);
                    cards.Remove(c);
                }
            DoorDeck.Cards.AddRange(cards.Where(x => x.Type == CardType.Dungeon));
            TreasureDeck.Cards.AddRange(cards.Where(x => x.Type == CardType.Treasure));
        }

        public void StepFinished()
        {
            /* ToDo: real algorythm for 4 ppl game
            int currentPlayerIndex = Players.FindIndex(0, p => p.UserId == CurrentPlayerId);
            if (currentPlayerIndex + 1 > PLAYERS_COUNT)
                currentPlayerIndex = 0;
            else
                currentPlayerIndex++;
            CurrentPlayerId = Players.ElementAt(currentPlayerIndex).UserId;
            */
            int currentPlayerIndex = Players.FindIndex(0, p => p.UserId == CurrentPlayerId);
            do
            {
                if (Players.Count > currentPlayerIndex + 1)
                    currentPlayerIndex++;
                else
                    currentPlayerIndex = 0;
            }
            while (Players.ElementAt(currentPlayerIndex).UserId.ToString("D") == "00000000-0000-0000-0000-000000000000");
            CurrentPlayerId = Players.ElementAt(currentPlayerIndex).UserId;
        }
    }

    public enum TurnStep
    {
        Inital,
        Battle,
        Waiting,
        Ending
    }
}