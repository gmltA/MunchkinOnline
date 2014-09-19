using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string PassHash { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public string VkId { get; set; }

        [ScaffoldColumn(false)]
        public string VkHash { get; set; }

        [ScaffoldColumn(false)]
        public uint GamesPlayed { get; set; }

        [ScaffoldColumn(false)]
        public uint GamesWon { get; set; }

        public State State { get; set; }

        public Gender Gender { get; set; }

        public Role Role { get; set; }

        [ScaffoldColumn(false)]
        public DateTime LastActivity { get; set; }

        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<GameLogEntry> Games { get; set; }
     }

    public enum Role
    {
        Admin,
        Player
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