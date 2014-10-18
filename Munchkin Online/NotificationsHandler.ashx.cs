using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Munchkin_Online.Core;
using Munchkin_Online.Core.Notifications;
using System.Diagnostics;

namespace Munchkin_Online
{
    /// <summary>
    /// Notification-related requests handler
    /// </summary>
    public class NotificationsHandler : IHttpAsyncHandler, IRequiresSessionState
    {
        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext ctx, AsyncCallback cb, Object obj)
        {
            ClientState currentAsyncState = new ClientState(ctx, cb, obj);

            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestWorker), currentAsyncState);

            return currentAsyncState;
        }

        public void EndProcessRequest(IAsyncResult ar)
        {
        }

        #endregion

        /// <summary>
        /// Обработчик сообщения от клиента
        /// </summary>
        /// <param name="obj">Состояние клиента</param>
        private void RequestWorker(Object obj)
        {
            ClientState state = obj as ClientState;

            string action = state.CurrentContext.Request.QueryString["action"];

            if (!NotificationManager.Instance.CheckClient(state))
            {
                NotificationManager.Instance.RegisterClient(state);

                state.CompleteRequest();
                return;
            }

            switch (action)
            {
                case "unregister":
                    state.CompleteRequest();
                    break;
                case "demandNotifications":
                    string result = NotificationManager.Instance.GetContent(state);
                    state.CurrentContext.Response.Write(result);
                    state.CompleteRequest();
                    break;
                default:
                    Debug.WriteLine("Unexpected action type in NotificationsHandler: {0}", action);
                    break;

            }
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
        }

        #endregion
    }
}