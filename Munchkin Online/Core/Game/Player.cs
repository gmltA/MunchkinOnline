using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Core.Game.Cards;
using Munchkin_Online.Core.Game.Mechanics;

namespace Munchkin_Online.Core.Game
{
    public class Player : CardHolder, ITarget
    {
        public Guid UserId { get; set; }

        public int Level { get; set; }
        public int GamesPlayed { get; set; }

        public CardHolder Hand { get; set; }
        public CardHolder Board { get; set; }

        public bool IsConfirmed { get; set; }

        public Race Race { get; set; }
        public Class Class { get; set; }

        public Player()
        {
            Level = 1;
            Hand = new CardHolder();
            Board = new CardHolder();
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

        internal bool TryEquip(Card card)
        {
            if (card == null)
                return false;

            if (card.Class == CardClass.Class || card.Class == CardClass.Race)
            {
                int cardCount = Board.Cards.Count(c => c.Class == card.Class) + 1;
                int maxCards = 1;

                var combo = CardClass.ClassCombo;
                if (card.Class == CardClass.Race)
                    combo = CardClass.RaceCombo;

                if (Board.Cards.Count(c => c.Class == combo) > 0)
                    maxCards = 2;

                if (cardCount > maxCards)
                    return false;
            }
            else if (card.Class == CardClass.ClassCombo || card.Class == CardClass.RaceCombo)
            {
                if (Board.Cards.Count(c => c.Class == card.Class) > 0)
                    return false;
            }
            return true;
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