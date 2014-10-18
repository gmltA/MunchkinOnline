﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game;

namespace Munchkin_Online.Core.Game
{
    public class Match
    {
        public event EventHandler MatchEnded = delegate { };


        public Guid Id { get; set; }

        public Player Creator { get; set; }

        public virtual List<Player> Players { get; set; }

        public State State { get; set; }

        public Player Winner { get; set; }

        public BoardState BoardState { get; set; }

        public Match()
        {
            Players = new List<Player>();
        }
    }

    public enum State
    {
        Lobby,
        InGame,
        Ended
    }
}