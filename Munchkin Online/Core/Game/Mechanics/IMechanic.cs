using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Game.Mechanics
{
    public abstract class IMechanic
    {
        public const string ACTION_DONE = "OK";
        public const string ACTION_ERROR = "ERROR";

        public string Execute(BoardState state, ITarget target);

    }
}