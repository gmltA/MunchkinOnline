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

        /// <summary>
        /// Время, через которое будет принудительно создана игра при отсутствии новых игроков
        /// </summary>
        public const int CREATE_INTERVAL = 120000;
        
        public static readonly Matchmaking Instance = new Matchmaking();

        public List<User> Users { get; set; }
        public List<Match> Matches { get; set; }
        public int LastCount { get; set; }
        public Timer Timer { get; set; }

        Matchmaking()
        {
            LastCount = 0;
            Users = new List<User>();
            LongPoolHandler.NewSearcher += OnNewSearcher;
            Timer = new Timer(CREATE_INTERVAL);
            ResetTimer();
        }

        /// <summary>
        /// Обработчик события появления нового человека в поиске
        /// </summary>
        /// <param name="sender">Собственно User(будет, а пока ClientState)</param>
        /// <param name="e">//TODO: </param>
        public void OnNewSearcher(object sender, NewFinderArgs e)
        {
            Users.Add(sender as User);
            ResetTimer();
            CalculateMatches();
        }

        /// <summary>
        /// Метод, распределяющий игроков по матчам
        /// </summary>
        void CalculateMatches()
        {

            if (Users.Count < 4)
                return;

            if (LastCount == Users.Count)
            {
                var UsersForMatch = Users.Take(4);
                CreateMatch(UsersForMatch);
            }
            else
            {
                var dict = Users.GroupBy(x => x.GamesPlayed);
                foreach (var key in dict)
                {
                    if (key.Count() > 4)
                        CreateMatch(key.Take(4));
                }
            }
            LastCount = Users.Count;
        }


        /// <summary>
        /// Перезагрузка будильника
        /// </summary>
        void ResetTimer()
        {
            Timer.Stop();
            Timer.Close();
            Timer = new Timer(CREATE_INTERVAL);
            Timer.AutoReset = false;
            Timer.Elapsed += (x, y) => CalculateMatches();
            Timer.Start();
        }

        void CreateMatch(IEnumerable<User> UsersForMatch)
        {
            List<Player> Players = new List<Player>();
            foreach (var i in UsersForMatch)
            {
                Players.Add(new Player(i));
                Users.Remove(i);
            }
            Match match = new Match();
            match.Players = Players;
            match.MatchEnded += OnMatchEnded;
            Matches.Add(match);
            MatchCreated(this, new MatchCreatedArgs());
        }

        /// <summary>
        /// Обработчик конца матча
        /// </summary>
        /// <param name="sender">Матч, который завершился.</param>
        /// <param name="e">Пустой параметр</param>
        void OnMatchEnded(object sender, EventArgs e)
        {
            Match m = sender as Match;
            m.State = Game.State.Ended;
            foreach (var p in m.Players)
            {
                ///TODO: User.GamesPlayed++;
            }
            ///TODO: Winner.GamesWon++;
            ///TODO: Add stats to db(optionally);
            Matches.Remove(m);
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