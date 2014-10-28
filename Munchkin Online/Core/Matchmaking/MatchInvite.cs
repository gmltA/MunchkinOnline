using Munchkin_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Matchmaking
{
    public class MatchInvite
    {
        public Guid Id { get; private set; }

        public Guid MatchId { get; private set; }
        public Guid UserToInviteId { get; private set; }
        public User InvitingUser { get; private set; }

        public DateTime InviteDate { get; private set; }

        public MatchInvite(Guid matchId, User invitingUser, Guid userToInviteId)
        {
            Id = Guid.NewGuid();
            MatchId = matchId;
            UserToInviteId = userToInviteId;
            InvitingUser = invitingUser;
            InviteDate = DateTime.Now;
        }
    }
}