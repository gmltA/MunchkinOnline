using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Battle
    {
        public Player InitPlayer { get; set; }
        public List<Player> Players { get; set; }

        public Card Monster { get; set; }

        public List<Card> AdditionalCards { get; set; }
    }
}