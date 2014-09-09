using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Player
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int Level { get; set; }

        public virtual ICollection<Card> Hand { get; set; }
        public virtual ICollection<Card> Board { get; set; } 
    }
}