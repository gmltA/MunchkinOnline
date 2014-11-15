using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class Card
    {
        const string BasePath = "/Content/img/cards/";

        const string Ext = ".png";

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CardType Type { get; set; }

        public CardClass Class { get; set; }

        public bool IsBig { get; set; }

        public bool IsTwoHanded { get; set; }

        public bool IsActive { get; set; }

        public bool IsCheated { get; set; }

        public bool IsUsableOnce { get; set; }

        public int Cost { get; set; }

        public int Treasures { get; set; }

        public int Levels { get; set; }

        public string Path { get { return BasePath + Name + Ext; } }

        public virtual ICollection<Mechanic> Mechanics { get; set; }

    }

    public enum CardType
    {
        Dungeon,
        Treasure
    }

    public enum CardClass
    {
        Item,
        Monster,
        Spell,
        Class,
        Race,
        Headgear,
        Footgear,
        Armor
    }
    
}