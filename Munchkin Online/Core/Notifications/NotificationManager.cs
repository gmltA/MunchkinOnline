using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Script.Serialization;

namespace Munchkin_Online.Core.Notifications
{
    public class NotificationManager
    {
        private const string guidCookieName = "__NOTIF_GUID_COOKIE";
        private const string tokenCookieName = "__NOTIF_TOKEN_COOKIE";

        /**
         * ToDo: Consider intervals, testing
         */
        private const int CLEANUP_INTERVAL = 6000;
        private const int DELAY_BEFORE_CLEANUP = 3600000;

        public static readonly NotificationManager Instance = new NotificationManager();
        public Dictionary<Guid, TimedList<Notification>> NotificationStore { get; set; }

        public Timer CleanupTimer { get; set; }

        NotificationManager()
        {
            NotificationStore = new Dictionary<Guid, TimedList<Notification>>();
            CleanupTimer = new Timer(CLEANUP_INTERVAL);
            ResetTimer();
        }

        public void RegisterClient(ClientState state)
        {
            Guid clientGuid = Guid.NewGuid();
            string guidStr = clientGuid.ToString("N");
            state.ClientGuid = guidStr;
            state.CurrentContext.Response.Cookies.Set(new HttpCookie(guidCookieName, guidStr));
            state.CurrentContext.Response.Cookies.Set(new HttpCookie(tokenCookieName, GenerateToken(state, guidStr)));
        }

        public bool CheckClient(ClientState state)
        {
            HttpCookie notificationClientCookie = state.CurrentContext.Request.Cookies.Get(guidCookieName);
            HttpCookie notificationClientTokenCookie = state.CurrentContext.Request.Cookies.Get(tokenCookieName);
            if (notificationClientCookie != null && !string.IsNullOrEmpty(notificationClientCookie.Value) &&
                notificationClientTokenCookie != null && !string.IsNullOrEmpty(notificationClientTokenCookie.Value))
            {
                if (string.Compare(notificationClientTokenCookie.Value, (GenerateToken(state, notificationClientCookie.Value)), true) == 0)
                {
                    state.ClientGuid = notificationClientCookie.Value;
                    return true;
                }
            }
            return false;
        }

        private string GenerateToken(ClientState state, string guid)
        {
            string userAgent = state.CurrentContext.Request.UserAgent;
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(userAgent + ":" + guid))).Replace("-", "");
        }

        public void Add(string message, NotificationType type)
        {
            HttpCookie notificationClientCookie = HttpContext.Current.Request.Cookies.Get(guidCookieName);
            Guid clientGuid = new Guid(notificationClientCookie.Value);
            if (!NotificationStore.ContainsKey(clientGuid))
                NotificationStore[clientGuid] = new TimedList<Notification>();

            NotificationStore[clientGuid].Add(new Notification(message, type));
        }

        public string GetContent(ClientState state)
        {
            Guid clientGuid = new Guid(state.ClientGuid);
            if (!NotificationStore.ContainsKey(clientGuid))
                return "";

            string result = new JavaScriptSerializer().Serialize(NotificationStore[clientGuid]);
            NotificationStore[clientGuid].Clear();
            return result;
        }

        void ResetTimer()
        {
            CleanupTimer.Stop();
            CleanupTimer.Close();
            CleanupTimer = new Timer(CLEANUP_INTERVAL);
            CleanupTimer.AutoReset = true;
            CleanupTimer.Elapsed += (x, y) => StoreCleanup();
            CleanupTimer.Elapsed += (x, y) => ResetTimer();
            CleanupTimer.Start();
        }

        private void StoreCleanup()
        {
            foreach (var list in NotificationStore)
            {
                if (list.Value.LastUpdate.AddMilliseconds(DELAY_BEFORE_CLEANUP) < DateTime.Now)
                    NotificationStore.Remove(list.Key);
            }
        }
    }
}