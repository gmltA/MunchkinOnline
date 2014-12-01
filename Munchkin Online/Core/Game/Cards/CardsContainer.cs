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

            Cards.Add(InstanateSpell("Friendship Potion"));

            Cards.Add(InstanateMonster("Nuclear Dragon"));
        }

        static Card InstanateRace(string name)
        {
            Card card = new Card();
            card.Id = Cards.Count + 1;
            card.Name = name;
            card.Type = CardType.Dungeon;
            card.Mechanics = new List<IMechanic>();
            card.Mechanics.Add(new SetRaceMechanic());
            card.Class = CardClass.Race;
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
            return card;
        }

        static Card InstanateSpell(string name)
        {
            Card card = new Card();
            card.Id = Cards.Count + 1;
            card.Name = name;
            card.Type = CardType.Treasure;
            card.Mechanics = new List<IMechanic>();
            card.Mechanics.Add(new EvadeMonstersMechanic());
            card.Class = CardClass.Spell;
            return card;
        }

        static Card InstanateMonster(string name)
        {
            Card card = new Card();
            card.Id = Cards.Count + 1;
            card.Name = name;
            card.Type = CardType.Dungeon;
            card.Mechanics = new List<IMechanic>();
            card.Mechanics.Add(new KillPlayerMechanic());
            card.Class = CardClass.Monster;
            return card;
        }
    }
}