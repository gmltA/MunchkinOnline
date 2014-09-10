using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CardTypes Type { get; set; }

        public CardClasses Class { get; set; }

        public bool IsBig { get; set; }

        public bool IsTwohanded { get; set; }

        public bool IsActive { get; set; }

        public bool IsCheated { get; set; }

        public bool IsOnceUsing { get; set; }

        public int Cost { get; set; }

        public int Treasures { get; set; }

        public int Levels { get; set; }

        public virtual ICollection<Mechanic> Mechanics { get; set; }

        enum CardTypes
        {
            Dungeon,
            Treasure
        }

        enum CardClasses
        {
            Item,
            Monster,
            Spell,
            Class,
            Race
        }
    }
}