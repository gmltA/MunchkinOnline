using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        static Card InstanateRace(string name, params Mechanic[] mechanics)
        {
            Card a = new Card();
            a.Id = Cards.Count + 1;
            a.Name = name;
            a.Type = CardType.Dungeon;
            //a.Mechanics = new List<Mechanic>(mechanics);
            a.Class = CardClass.Race;
            return a;
        }

        static Card InstanateClass(string name, params Mechanic[] mechanics)
        {
            Card a = new Card();
            a.Id = Cards.Count + 1;
            a.Name = name;
            a.Type = CardType.Dungeon;
            //a.Mechanics = new List<Mechanic>(mechanics);
            a.Class = CardClass.Class;
            return a;
        }
    }
}