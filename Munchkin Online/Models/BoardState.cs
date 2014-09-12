using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class BoardState
    {
        public const int PLAYERS_COUNT = 4;

        public List<Card> DoorDeck { get; set; }
        public List<Card> DoorTrash { get; set; }

        public List<Card> TreasureDeck { get; set; }
        public List<Card> TreasureTrash { get; set; }

        public int CurrentPlayerId { get; set; }
        public List<Player> Players { get; set; }

        public TurnStep TurnStep { get; set; }
        public Battle Battle { get; set; }
    }

    public enum TurnStep
    {
        Inital,
        Battle,
        CleaningNychki,
        Ending
    }
}