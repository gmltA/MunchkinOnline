using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game.Mechanics;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Game.Cards
{
    public static class CardsContainer
    {
        static List<Card> Cards;

        public static List<Card> GetCards()
        {
            return new List<Card>(Cards);
        }

        static CardsContainer()
        {
            Cards = new List<Card>();
            Cards.Add(InstanateRace("Dwarf"));
            Cards.Add(InstanateRace("Elf"));
            Cards.Add(InstanateRace("Huffling"));
            Cards.Add(InstanateRace("Dwarf"));
            Cards.Add(InstanateRace("Elf"));
            Cards.Add(InstanateRace("Huffling"));

            Cards.Add(InstanateClass("Warrior"));
            Cards.Add(InstanateClass("Mage"));
            Cards.Add(InstanateClass("Thief"));
            Cards.Add(InstanateClass("Warrior"));
            Cards.Add(InstanateClass("Mage"));
            Cards.Add(InstanateClass("Thief"));
        }

        static Card InstanateRace(string name)
        {
            Card card = new Card();
            card.Id = Cards.Count + 1;
            card.Name = name;
            //todo: treasure -> dungeoun
            card.Type = CardType.Treasure;
            card.Mechanics = new List<IMechanic>();
            card.Mechanics.Add(new SetRaceMechanic());
            card.Class = CardClass.ClassCombo;
            card.CSSClass = "plutonium-dragon";
            return card;
        }

        static Card InstanateClass(string name)
        {
            Card card = new Card();
            card.Id = Cards.Count + 1;
            card.Name = name;
            card.Type = CardType.Dungeon;
            card.Mechanics = new List<IMechanic>();
            card.Mechanics.Add(new SetRaceMechanic());
            card.Class = CardClass.Class;
            card.CSSClass = "plutonium-dragon";
            return card;
        }
    }
}