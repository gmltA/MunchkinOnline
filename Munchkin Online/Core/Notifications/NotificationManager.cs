using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Munchkin_Online.Core.Notifications
{
    public class NotificationManager
    {
        private const string guidCookieName = "__NOTIF_GUID_COOKIE";
        private const string tokenCookieName = "__NOTIF_TOKEN_COOKIE";

        public static readonly NotificationManager Instance = new NotificationManager();
        public Dictionary<Guid, List<Notification>> NotificationStore { get; set; }

        NotificationManager() { NotificationStore = new Dictionary<Guid, List<Notification>>(); }

        public void RegisterClient(ClientState state)
        {
            Guid clientGuid = Guid.NewGuid();
            string guidStr = clientGuid.ToString("N");
            state.ClientGuid = guidStr;
            state.CurrentContext.Response.Cookies.Set(new HttpCookie(guidCookieName, guidStr));
            state.CurrentContext.Response.Cookies.Set(new HttpCookie(tokenCookieName, GenerateToken(state, guidStr)));
            if (NotificationStore.Where(x => x.Key == clientGuid).Count() == 0)
                NotificationStore[clientGuid] = new List<Notification>();
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
                    Guid clientGuid = new Guid(state.ClientGuid);
                    if (NotificationStore.Where(x => x.Key == clientGuid).Count() == 0)
                        NotificationStore[clientGuid] = new List<Notification>();
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
            NotificationStore[new Guid(notificationClientCookie.Value)].Add(new Notification(message, type));
        }

        public string GetContent(ClientState state)
        {
            Guid clientGuid = new Guid(state.ClientGuid);
            string result = new JavaScriptSerializer().Serialize(NotificationStore[clientGuid]);
            NotificationStore[clientGuid].Clear();
            return result;
        }
    }
}