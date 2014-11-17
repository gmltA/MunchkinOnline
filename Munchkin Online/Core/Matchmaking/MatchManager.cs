using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Longpool;
using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Munchkin_Online.Core.Matchmaking
{
    public class MatchManager
    {
        public static readonly MatchManager Instance = new MatchManager();

        public List<Match> Matches { get; set; }
        public List<MatchInvite> MatchInvites { get; set; }

        public const int INVITE_CLEANUP_INTERVAL = 120000;
        public Timer InviteCleanupTimer { get; set; }

        MatchManager()
        {
            Matches = new List<Match>();
            MatchInvites = new List<MatchInvite>();

            InviteCleanupTimer = new Timer(INVITE_CLEANUP_INTERVAL);
            ResetTimer();
        }

        void ResetTimer()
        {
            InviteCleanupTimer.Stop();
            InviteCleanupTimer.Close();
            InviteCleanupTimer = new Timer(INVITE_CLEANUP_INTERVAL);
            InviteCleanupTimer.AutoReset = true;
            InviteCleanupTimer.Elapsed += (x, y) => CleanupInvites();
            InviteCleanupTimer.Start();
        }

        void CleanupInvites()
        {
            MatchInvites.RemoveAll(i => i.InviteDate.AddMilliseconds(INVITE_CLEANUP_INTERVAL) < DateTime.Now);
        }

        public Match CreateMatch()
        {
            Match match = new Match();
            match.Id = Guid.NewGuid();
            Matches.Add(match);
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

                match.State = Munchkin_Online.Core.Game.State.Lobby;
                match.Creator = new Player(user);
                match.Players.Add(new Player(user));
            }

            return match;
        }

        public MatchInvite InviteUserToLobby(User invitingUser, Guid userToInviteId)
        {
            Match lobby = MatchManager.Instance.FindMatchByParticipantID(invitingUser.Id, true);
            if (lobby == null)
                return null;

            MatchInvite invite = new MatchInvite(lobby.Id, invitingUser, userToInviteId);
            MatchManager.Instance.MatchInvites.Add(invite);
            return invite;
        }

        public bool KickUserFromLobby(User invitingUser, Guid userToInviteId)
        {
            Match lobby = MatchManager.Instance.FindMatchByParticipantID(invitingUser.Id, true);
            if (lobby == null)
                return false;

            UserLeaveFromMatch(userToInviteId);
            return true;
        }

        public List<Guid> GetInvitedUsersIdByUserId(Guid invitingUserId)
        {
            return MatchInvites.Where(i => i.InvitingUser.Id == invitingUserId).Select(u => u.UserToInviteId).ToList();
        }

        public List<Guid> GetExcludeFromInviteUserIdList(Guid invitingUserId)
        {
            Match lobby = MatchManager.Instance.FindMatchByParticipantID(invitingUserId, true);
            if (lobby == null)
                return new List<Guid>();

            var alreadyInLobbyUserIdList = lobby.Players.Select(u => u.UserId).ToList();
            var alreadyInvitedUsersIdList = MatchManager.Instance.GetInvitedUsersIdByUserId(invitingUserId);

            List<Guid> result = new List<Guid>();
            result.AddRange(alreadyInLobbyUserIdList);
            result.AddRange(alreadyInvitedUsersIdList);

            return result;
        }

        public void CleanupInvitesForUserId(Guid invitinguserId)
        {
            MatchInvites.RemoveAll(i => i.InvitingUser.Id == invitinguserId);
        }

        public void UserLeaveFromMatch(Guid userId)
        {
            Match match = FindMatchByParticipantID(userId);
            if (match != null)
            {
                if (match.Creator.UserId == userId)
                {
                    match.SendMessageToPlayers(new AsyncMessage(MessageType.LobbyUpdate), true);
                    CleanupInvitesForUserId(userId);
                    Matches.Remove(match);
                }
                else
                {
                    match.Players.RemoveAll(p => p.UserId == userId);
                    match.SendMessageToPlayers(new AsyncMessage(MessageType.LobbyUpdate));
                }
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

            match.SendMessageToPlayers(new AsyncMessage(MessageType.LobbyUpdate));
            match.Players.Add(new Player(user));
        }

        public bool UserStartsMatch(Guid userId)
        {
            Match match = MatchManager.Instance.FindMatchByParticipantID(userId, true);
            if (match == null)
                return false;

            match.Start();
            return true;
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