﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace Munchkin_Online.Core.Database
{
    public class UserRepository : IDisposable
    {
        MainContext DB = new MainContext();

        public IQueryable<User> Accounts
        {
            get
            {
                return DB.Users;
            }
        }

        public bool Add(User instance)
        {
            var user = GetUser(instance.Email);
            if (user != null)
                return false;
            try
            {
                instance.Id = Guid.NewGuid();
                instance.PasswordHash = PasswordCryptor.Crypt(instance.PasswordHash);
                DB.Users.Add(instance);
                DB.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public User Login(string email, string pass)
        {
            var user = DB.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0 && string.Compare(p.PasswordHash, pass, false) == 0);
            if(user != null)
            {
                user.LastActivity = DateTime.Now;
                DB.SaveChanges();
            }
            return user;
        }

        public User Login(int vkId, string email)
        {
            var user = DB.Users.FirstOrDefault(p => p.VkId == vkId && string.Compare(p.Email, email, true) == 0);
            if (user != null)
            {
                user.LastActivity = DateTime.Now;
                DB.SaveChanges();
            }
            return user;
        }

        public User GetUser(string email)
        {
            return DB.Users.FirstOrDefault(p => string.Compare(p.Email, email, true) == 0);
        }

        public User GetUser(Guid userGuid)
        {
            return DB.Users.FirstOrDefault(p => p.Id == userGuid);
        }

        public User GetUserByVkId(int vkId)
        {
            return DB.Users.FirstOrDefault(p => p.VkId == vkId);
        }

        public List<UserFriendData> GetPotentialFriendListByNickname(string nicknamePart, User sender)
        {
            var result = DB.Users.Where(p => p.Nickname.Contains(nicknamePart) && p.Id != sender.Id)
                            .Select(f => new UserFriendData
                            {
                                ID = f.Id,
                                Nickname = f.Nickname,
                                GamesPlayed = f.GamesPlayed,
                                GamesWon = f.GamesWon,
                            }).ToList();
            foreach (var friend in result)
            {
                friend.FriendAlready = sender.Friends.Any(f => f.Id == friend.ID);
            }
            return result;
        }

        public void ForceSaveChanges()
        {
            DB.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            IDisposable d = DB as IDisposable;
            if (d != null)
                d.Dispose();
            GC.SuppressFinalize(this);
        }

        internal List<TopPlayersNode> GetTopPlayers()
        {
            return DB.Users
                .Select<User, TopPlayersNode>( x => new TopPlayersNode
                                                {
                                                    Id = x.Id,
                                                    Username = x.Nickname,
                                                    Winrate = x.GamesPlayed==0?0:(float)x.GamesWon/x.GamesPlayed
                                                }).OrderBy(x=>x.Winrate).Take(10).ToList();
        }
    }

    public class UserFriendData
    {
        public Guid ID { get; set; }
        public string Nickname { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public bool FriendAlready { get; set; }
    }
}