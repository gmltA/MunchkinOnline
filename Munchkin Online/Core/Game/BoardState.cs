using System;
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
        public const int DOORS_COUNT = 1;
        public const int TREASURES_COUNT = 0;

        public List<Card> DoorDeck { get; set; }
        public List<Card> DoorTrash { get; set; }

        public List<Card> TreasureDeck { get; set; }
        public List<Card> TreasureTrash { get; set; }

        public int CurrentPlayerId { get; set; }
        public List<Player> Players { get; set; }

        public TurnStep TurnStep { get; set; }
        public Battle Battle { get; set; }

        public BoardState(List<Player> players)
        {
            Players = players;
            DoorDeck = new List<Card>();
            DoorTrash = new List<Card>();
            
            TreasureDeck = new List<Card>();
            TreasureTrash = new List<Card>();
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
                    p.Hand.Add(c);
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
                    p.Hand.Add(c);
                    cards.Remove(c);
                }
            DoorDeck = cards.Where(x => x.Type == CardType.Dungeon).ToList();
            TreasureDeck = cards.Where(x => x.Type == CardType.Treasure).ToList();
        }
    }

    public enum TurnStep
    {
        Inital,
        Battle,
        CleaningNychki,
        Ending
    }
}