using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Core.Game.Cards;

namespace Munchkin_Online.Core.Game
{
    public class Player : CardHolder
    {
        public Guid UserId { get; set; }

        public int Level { get; set; }
        public int GamesPlayed { get; set; }

        public List<Card> Hand { get; set; }
        public List<Card> Board { get; set; }

        public bool IsConfirmed { get; set; }

        public Race Race { get; set; }
        public Class Class { get; set; }

        public Player()
        {
            Level = 1;
            Hand = new List<Card>();
            Board = new List<Card>();
        }

        public Player(User user) : this()
        {
            UserId = user.Id;
            IsConfirmed = false;
            GamesPlayed = user.GamesPlayed;
        }

        public User ToUser()
        {
            UserRepository repo = new UserRepository();
            return repo.Accounts.Where(u => u.Id == this.UserId).FirstOrDefault();
        }

        public Card GetCardById(int cardId)
        {
            return Hand.Where(c => c.Id == cardId).FirstOrDefault();
        }

        public bool AddCard(int cardId)
        {
            if (GetCardById(cardId) != null)
                return false;

            Card card = CardsContainer.GetCards().Where(c => c.Id == cardId).FirstOrDefault();
            if (card == null)
                return false;

            Hand.Add(card);
            return true;
        }

        public bool AddCard(Card card)
        {
            if (GetCardById(card.Id) != null)
                return false;

            Hand.Add(card);
            return true;
        }

        public bool RemoveCard(int cardId)
        {
            Card playerCard = GetCardById(cardId);
            if (playerCard == null)
                return false;

            Hand.Remove(playerCard);
            return true;
        }

        public bool RemoveCard(Card card)
        {
            Card playerCard = GetCardById(card.Id);
            if (playerCard == null)
                return false;

            Hand.Remove(playerCard);
            return true;
        }

        public Card GetRandomCard()
        {
            Random random = new Random();
            return Hand.ElementAt(random.Next(Hand.Count));
        }
    }

    public enum Class
    {
        No,
        Warrior,
        Mage,
        Thief
    }

    public enum Race
    {
        Human,
        Dwarf,
        Elf,
        Huffling
    }
}