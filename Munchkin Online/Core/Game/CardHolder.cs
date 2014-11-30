using Munchkin_Online.Core.Game.Cards;
using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munchkin_Online.Core.Game
{
    public class CardHolder
    {
        public List<Card> Cards { get; set; }

        public CardHolder()
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
            if (card == null)
                return false;

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
            if (card == null)
                return false;

            Card playerCard = GetCardById(card.Id);
            if (playerCard == null)
                return false;

            Cards.Remove(playerCard);
            return true;
        }

        public Card GetRandomCard()
        {
            if (Cards.Count == 0)
                return null;

            Random random = new Random();
            return Cards.ElementAt(random.Next(Cards.Count));
        }
    }
}
