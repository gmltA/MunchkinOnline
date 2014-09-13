using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Models;

namespace Munchkin_Online.Core.Matchmaking
{
    /// <summary>
    /// Класс содержит логику работы мм
    /// </summary>
    public class Matchmaking
    {
        /// <summary>
        /// Событие создания игры
        /// </summary>
        public event EventHandler<MatchCreatedArgs> MatchCreated = delegate { };

        public static readonly Matchmaking Instance = new Matchmaking();

        public List<User> Users { get; set; }
        public DateTime LastFinder { get; set; }
        public int LastCount { get; set; }

        Matchmaking()
        {
            LastCount = 0;
            Users = new List<User>();
            LongPoolHandler.NewFinder += OnNewFinder;
            Timer t = new Timer(120000);
            t.AutoReset = true;
            t.Elapsed += (x, y) => CalculateMatches();
        }

        /// <summary>
        /// Обработчик события появления нового человека в поиске
        /// </summary>
        /// <param name="sender">Собственно User</param>
        /// <param name="e">//TODO: </param>
        public void OnNewFinder(object sender, NewFinderArgs e)
        {
            Users.Add(sender as User);
            LastFinder = DateTime.Now;
            CalculateMatches();
        }

        void CalculateMatches()
        {

            if (Users.Count < 4)
                return;

            if (LastCount == Users.Count)
            {
                var UsersForMatch = Users.Take(4);
                List<Player> Players = new List<Player>();
                foreach (var i in UsersForMatch)
                {
                    Players.Add(new Player(i));
                }
                Match match = new Match();
            }
            else
            {

            }
           
        }
    }

    public class MatchCreatedArgs : EventArgs
    {
        public Match Match { get; set; }

        public MatchCreatedArgs()
        {
            
        }


    }
}