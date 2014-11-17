using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Auth;

namespace Munchkin_Online.Core.Game
{
    public class BoardStateInfo
    {
        public const int PLAYERS_COUNT = 4;

        public Guid CurrentPlayerId { get; set; }
        public List<PlayerInfo> Players { get; set; }
        public Player Me { get; set; }

        public TurnStep TurnStep { get; set; }
        public Battle Battle { get; set; }

        public BoardStateInfo(BoardState b)
        {
            CurrentPlayerId = b.CurrentPlayerId;
            Players = new List<PlayerInfo>();
            foreach (var p in b.Players)
            {
                if (p.UserId == CurrentUser.Instance.Current.Id)
                    Me = p;
                else
                    Players.Add(new PlayerInfo(p));
            }
            TurnStep = b.TurnStep;
            Battle = b.Battle;
        }
    }
}