using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Longpool;

namespace Munchkin_Online.Core.Game
{
    public class Match
    {
        public event EventHandler MatchEnded = delegate { };

        public Guid Id { get; set; }

        public Player Creator { get; set; }

        public virtual List<Player> Players { get; set; }

        public State State { get; set; }

        public Player Winner { get; set; }

        public BoardState BoardState { get; set; }

        public Match()
        {
            State = State.Created;
            Players = new List<Player>();
        }

        public void Start()
        {
            if (State != State.Lobby && State != State.Created)
                throw new WrongMatchStateException();

            // todo: define somewhere
            if (Players.Count != 4)
            {
                for (int i = Players.Count; i < 4; i++)
                    Players.Add(new Player());
                //throw new NotEnoughPlayersException();
            }

            State = State.InGame;

            BoardState = new BoardState(Players);
            SetRandomStartPlayer();
            SendMessageToPlayers(new AsyncMessage(MessageType.GameStarted));
        }

        public void SendMessageToPlayers(AsyncMessage message, bool exceptCreator = false)
        {
            foreach (Player player in Players)
            {
                if (!exceptCreator || (exceptCreator && player.UserId != Creator.UserId))
                    Longpool.Longpool.Instance.PushMessageToUser(player.UserId, message);
            }
        }

        public void SetRandomStartPlayer()
        {
            Random r = new Random();
            var actualPlayers = Players.Where(p => p.UserId != Guid.Empty).ToList();
            BoardState.CurrentPlayerId = actualPlayers[r.Next(actualPlayers.Count-1)].UserId;
            BoardState.TurnStep = TurnStep.Inital;
        }
    }

    public enum State
    {
        Created,
        Lobby,
        InGame,
        Ended
    }

    public class MatchStartException : Exception { }

    public class WrongMatchStateException : MatchStartException { }
    public class NotEnoughPlayersException : MatchStartException { }
}