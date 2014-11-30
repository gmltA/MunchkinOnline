using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Game
{
    public class ActionInfo
    {
        public Player Invoker { get; set; }

        public int CardId { get; set; }

        public ActionEntryType SourceEntry { get; set; }
        public int SourceParam { get; set; }
        public CardHolder Source { get; set; }

        public ActionEntryType TargetEntry { get; set; }
        public int TargetParam { get; set; }
        public CardHolder Target { get; set; }
    }

    public enum ActionEntryType
    {
        Deck,
        Slot,
        Card,
        Hand,
        Trash,
        Field,
        Player
    }
}