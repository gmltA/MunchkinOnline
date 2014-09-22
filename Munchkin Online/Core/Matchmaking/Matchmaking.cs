using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using Munchkin_Online.Core.Game;
using Munchkin_Online.Core.Longpool;
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
        public const int CONFIRM_WAIT_INTERVAL = 10000;

        public static readonly Matchmaking Instance = new Matchmaking();

        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
        public int LastCount { get; set; }
        public Timer Timer { get; set; }

        Matchmaking()
        {
            LastCount = 0;
            Players = new List<Player>();
            Matches = new List<Match>();
            LongPoolHandler.NewSearcher += OnNewSearcher;
            LongPoolHandler.MatchConfirmation += OnMatchConfirmation;
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
            Player player = new Player(sender as User);
            if (Players.Where(x => x.UserId == player.UserId).Count() != 0)
                return;
            Players.Add(player);
            foreach (var p in Players)
                Longpool.Longpool.Instance.PushMessageToUser(p.UserId, new AsyncMessage(MessageType.Notification, Players.Count));
            ResetTimer();
            CalculateMatches();
        }

        /// <summary>
        /// Метод, распределяющий игроков по матчам
        /// </summary>
        void CalculateMatches()
        {

            if (Players.Count < 4)
                return;

            if (LastCount == Players.Count)
            {
                var PlayersForMatch = Players.Take(4);
                PrepareMatch(PlayersForMatch);
            }
            else
            {
                var dict = Players.GroupBy(x => x.GamesPlayed);
                foreach (var key in dict)
                {
                    if (key.Count() > 4)
                        PrepareMatch(key.Take(4));
                }
            }
            LastCount = Players.Count;
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

        public void OnMatchConfirmation(object sender, EventArgs e)
        {
            var player = Players.FirstOrDefault(x => x.UserId == ((User)sender).Id);
            if (player != null)
                player.IsConfirmed = true;
        }

        void PrepareMatch(IEnumerable<Player> PlayersForMatch)
        {
            foreach (var Player in PlayersForMatch)
            {
                Longpool.Longpool.Instance.PushMessageToUser(Player.UserId, new AsyncMessage(MessageType.FindedMatch));
            }
            Timer timer = new Timer(CONFIRM_WAIT_INTERVAL);
            timer.AutoReset = false;
            timer.Elapsed += delegate
            {
                if (PlayersForMatch.All(x => x.IsConfirmed == true))
                    CreateMatch(PlayersForMatch);
                else
                    foreach (var player in PlayersForMatch)
                    {
                        player.IsConfirmed = false;
                        Longpool.Longpool.Instance.PushMessageToUser(player.UserId, new AsyncMessage(MessageType.NoConfirm));
                    }
            };
        }

        void CreateMatch(IEnumerable<Player> PlayersForMatch)
        {
            List<Player> players = new List<Player>();
            foreach (var i in PlayersForMatch)
            {
                players.Add(i);
                Players.Remove(i);
            }
            Match match = new Match();
            match.Players = players;
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