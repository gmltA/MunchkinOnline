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
            Cards.Add(InsnantRace("Ogre", null));
            Cards.Add(InsnantRace("Elf", null));
            Cards.Add(InsnantRace("Human", null));
            Cards.Add(InsnantRace("Fu", null));
        }

        static Card InsnantRace(string name, params Mechanic[] mechanics)
        {
            Card a = new Card();
            a.Id = Cards.Count + 1;
            a.Name = name;
            a.Type = CardType.Dungeon;
            //a.Mechanics = new List<Mechanic>(mechanics);
            a.Class = CardClass.Race;
            return a;
        }
    }
}