using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Models
{
    public class User
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Username for authentication.
        /// Should not be displayed or changed after registration for security reasons.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User display name.
        /// Can be changed at any moment.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// TODO: It's better to store pass hash instead of password itself to keep it in secret.
        /// </summary>
        public string Password { get; set; }

        public string Email { get; set; }

        public State State { get; set; }

        public Gender Gender { get; set; }

        public Role Role { get; set; }

        public GameStats GameStats { get; set; }

        public DateTime LastActivity { get; set; }

        public virtual ICollection<User> Friends { get; set; }
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
        Idle,
        InLobby,
        InGame
    }

    public struct GameStats
    {
        public uint played;
        public uint won;
    }
}