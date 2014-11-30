using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class TopPlayersNode
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public float Winrate { get; set; }

    }
}