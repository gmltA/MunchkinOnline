using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munchkin_Online.Core.Game
{
    public interface CardHolder
    {
        bool AddCard(int cardId);
        bool AddCard(Card card);

        bool RemoveCard(int cardId);
        bool RemoveCard(Card card);

        Card GetRandomCard();
        Card GetCardById(int cardId);
    }
}
