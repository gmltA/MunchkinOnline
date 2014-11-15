using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game
{
    public class PlayerInfo
    {
        public Guid UserId { get; set; }

        public int Level { get; set; }

        public int TreasuresCount { get; set; }
        public int DoorsCount { get; set; }
        public List<Card> Board { get; set; }

        public PlayerInfo(Player p)
        {
            UserId = p.UserId;
            Level = p.Level;
            Board = p.Board;
            DoorsCount = p.Hand.Where(x=>x.Type == CardType.Dungeon).Count();
            TreasuresCount = p.Hand.Where(x => x.Type == CardType.Treasure).Count();
        }
    }
}