using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Match
    {
        public int Id { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public States State { get; set; }

        public Player? Winner { get; set; }

        public BoardState? BoardState { get; set; } 
    }

    enum States
    {
        Lobby,
        InGame,
        Ended
    }
}