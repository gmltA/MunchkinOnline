using Munchkin_Online.Core.Game;
using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Matchmaking
{
    public class MatchManager
    {
        public static readonly MatchManager Instance = new MatchManager();

        public List<Match> Matches { get; set; }

        MatchManager()
        {
            Matches = new List<Match>();
        }

        public Match CreateMatch()
        {
            Match match = new Match();
            match.Id = Guid.NewGuid();

            return match;
        }

        public Match FindMatchByParticipantID(Guid userId)
        {
            return Matches.Where(m => m.Players.Any(p => p.UserId == userId)).FirstOrDefault();
        }

        public Match GetOrCreateNewMatchForUser(User user)
        {
            Match match = FindMatchByParticipantID(user.Id);
            if (match == null)
            {
                match = CreateMatch();

                match.Creator = new Player(user);
                match.Players.Add(new Player(user));
                Matches.Add(match);
            }

            return match;
        }

        public void UserLeaveFromMatch(Guid userId)
        {
            Match match = FindMatchByParticipantID(userId);
            if (match != null)
            {
                if (match.Creator.UserId == userId)
                    Matches.Remove(match);
                else
                    match.Players.RemoveAll(p => p.UserId == userId);
            }
        }

        public void UserJoinLobby(User user, Guid? lobbyId)
        {
            if (!lobbyId.HasValue)
                throw new LobbyJoinException(LobbyJoinExceptionType.WrongLobbyId);

            Match match = MatchManager.Instance.FindMatchByParticipantID(user.Id);
            if (match != null)
                throw new LobbyJoinException(LobbyJoinExceptionType.AlreadyInMatch);

            match = MatchManager.Instance.Matches.Where(m => m.Id == lobbyId).FirstOrDefault();
            if (match == null)
                throw new LobbyJoinException(LobbyJoinExceptionType.WrongLobbyId);

            if (match.Players.Count > 3)
                throw new LobbyJoinException(LobbyJoinExceptionType.LobbyIsFull);

            match.Players.Add(new Player(user));
        }
    }

    public class LobbyJoinException : Exception
    {
        public LobbyJoinExceptionType Type { get; private set; }

        public LobbyJoinException(LobbyJoinExceptionType type)
        {
            Type = type;
        }
    }

    public enum LobbyJoinExceptionType
    {
        AlreadyInMatch,
        WrongLobbyId,
        LobbyIsFull
    }
}