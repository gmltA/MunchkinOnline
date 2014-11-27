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

        public Deck DoorDeck { get; set; }
        public List<Card> DoorTrash { get; set; }

        public Deck TreasureDeck { get; set; }
        public List<Card> TreasureTrash { get; set; }

        public Guid CurrentPlayerId { get; set; }
        public List<Player> Players { get; set; }

        public TurnStep TurnStep { get; set; }
        public Battle Battle { get; set; }

        public BoardState(List<Player> players)
        {
            Players = players;

            DoorDeck = new Deck();
            DoorTrash = new List<Card>();

            TreasureDeck = new Deck();
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
            DoorDeck.Cards.AddRange(cards.Where(x => x.Type == CardType.Dungeon));
            TreasureDeck.Cards.AddRange(cards.Where(x => x.Type == CardType.Treasure));
        }
    }

    public class Deck : CardHolder
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public Card GetCardById(int cardId)
        {
            return Cards.Where(c => c.Id == cardId).FirstOrDefault();
        }

        public bool AddCard(int cardId)
        {
            if (GetCardById(cardId) != null)
                return false;

            Card card = CardsContainer.GetCards().Where(c => c.Id == cardId).FirstOrDefault();
            if (card == null)
                return false;

            Cards.Add(card);
            return true;
        }

        public bool AddCard(Card card)
        {
            if (GetCardById(card.Id) != null)
                return false;

            Cards.Add(card);
            return true;
        }

        public bool RemoveCard(int cardId)
        {
            Card playerCard = GetCardById(cardId);
            if (playerCard == null)
                return false;

            Cards.Remove(playerCard);
            return true;
        }

        public bool RemoveCard(Card card)
        {
            Card playerCard = GetCardById(card.Id);
            if (playerCard == null)
                return false;

            Cards.Remove(playerCard);
            return true;
        }

        public Card GetRandomCard()
        {
            Random random = new Random();
            return Cards.ElementAt(random.Next(Cards.Count));
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