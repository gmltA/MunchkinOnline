﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game.Mechanics;

namespace Munchkin_Online.Models
{
    public class Card : ITarget
    {
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

        public string CSSClass { get { return Name.Replace(' ', '-').ToLower(); } }

        public virtual ICollection<IMechanic> OnUseMechanics { get; set; }
        public virtual ICollection<IMechanic> AfterUseMechanics { get; set; }

        public Card()
        {
            OnUseMechanics = new List<IMechanic>();
            AfterUseMechanics = new List<IMechanic>();
        }

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
        ClassCombo,
        Race,
        RaceCombo,
        Headgear,
        Footgear,
        Armor,
        WeaponOneHand,
        WeaponTwoHanded
    }
    
}