using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class User
    {
        public Guid Id { get; set; }

        /// <summary>
        /// User display name.
        /// Can be changed at any moment.
        /// </summary>
        public string Nickname { get; set; }

        [MinLength(6)]
        public string PasswordHash { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public int VkId { get; set; }

        [ScaffoldColumn(false)]
        public string VkAccessToken { get; set; }

        [ScaffoldColumn(false)]
        public int GamesPlayed { get; set; }

        [ScaffoldColumn(false)]
        public int GamesWon { get; set; }

        [NotMapped]
        public State State { get; set; }

        public Gender Gender { get; set; }

        public Role Role { get; set; }

        [ScaffoldColumn(false)]
        public DateTime LastActivity { get; set; }

        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<GameLogEntry> Games { get; set; }

        public User()
        {
            this.Friends = new HashSet<User>();
        }

        public static bool operator==(User u1, User u2)
        {
            if (((object)u1 == null && (object)u2 != null) || ((object)u1 != null && (object)u2 == null))
                return false;
            if ((object)u1 == null && (object)u2 == null)
                return true;
            else
                return u1.Id == u2.Id;
        }

        public static bool operator !=(User u1, User u2)
        {
            if (((object)u1 == null && (object)u2 != null) || ((object)u1 != null && (object)u2 == null))
                return true;
            if((object)u1 == null && (object)u2 == null)
                return false;
            else
                return u1.Id != u2.Id;
        }
     }

    public enum Role
    {
        Player = 0,
        Admin
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum State
    {
        Offline,
        Idle,
        InLobby,
        InGame
    }
}