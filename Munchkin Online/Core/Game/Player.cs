using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game
{
    public class Player
    {
        public Guid UserId { get; set; }

        public int Level { get; set; }
        public int GamesPlayed { get; set; }

        public List<Card> Hand { get; set; }
        public List<Card> Board { get; set; }

        public bool IsConfirmed { get; set; }

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
            GamesPlayed = (int)user.GamesPlayed;
        }
    }
}