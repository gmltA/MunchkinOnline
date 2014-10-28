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

        public Match FindMatchByParticipantID(Guid userId, bool isCreator = false)
        {
            if (isCreator)
                return Matches.Where(m => m.Creator.UserId == userId).FirstOrDefault();
            else
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
                throw new WrongLobbyIdException();

            Match match = MatchManager.Instance.FindMatchByParticipantID(user.Id);
            if (match != null)
                throw new AlreadyInMatchException();

            match = MatchManager.Instance.Matches.Where(m => m.Id == lobbyId).FirstOrDefault();
            if (match == null)
                throw new WrongLobbyIdException();

            if (match.Players.Count > 3)
                throw new LobbyIsFullException();

            Matchmaking.Instance.Players.RemoveAll(p => p.UserId == user.Id);
            match.Players.Add(new Player(user));
        }
    }

    public class LobbyJoinException : Exception
    {
    }

    public class AlreadyInMatchException : LobbyJoinException
    {
    }

    public class WrongLobbyIdException : LobbyJoinException
    {
    }

    public class LobbyIsFullException : LobbyJoinException
    {
    }
}