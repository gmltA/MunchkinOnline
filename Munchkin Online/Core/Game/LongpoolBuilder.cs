using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Longpool;

namespace Munchkin_Online.Core.Game
{
    public class LongpoolBuilder
    {
        public static AsyncMessage GetInitMessage(Match match)
        { 
            return new AsyncMessage(MessageType.BattleMessage, new { Board = new BoardStateInfo(match.BoardState) });
        }

    }
}