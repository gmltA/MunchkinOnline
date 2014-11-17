using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Game
{
    public class ActionInfo
    {
        public int CardId { get; set; }
        
        public int TargetId { get; set; }

        public TargetType TargetType { get; set; }
    }

    public enum TargetType
    {
        MyClass,
        MyRace,
        BoardCard,
        HandCard
    }
}