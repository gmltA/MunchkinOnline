using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Munchkin_Online.Core.Longpool;

namespace Munchkin_Online
{
    /// <summary>
    /// Кастомный обработчик HTTP запроса
    /// </summary>
    public class LongPoolHandler : IHttpAsyncHandler, IRequiresSessionState
    {
        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext ctx,
          AsyncCallback cb, Object obj)
        {
            ClientState currentAsyncState =
              new ClientState(ctx, cb, obj);

            ThreadPool.QueueUserWorkItem(new WaitCallback(RequestWorker),
               currentAsyncState);

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

            string command =
              state.CurrentContext.Request.QueryString["cmd"];
            string guid =
              state.CurrentContext.Request.QueryString["guid"];

            switch (command)
            {
                case "register":
                    Longpool.Instance.RegicterClient(state);
                    state.CurrentContext.Response.Write(
                      state.ClientGuid.ToString());
                    state.CompleteRequest();
                    break;
                case "unregister":
                    Longpool.Instance.UnregisterClient(state);
                    state.CompleteRequest();
                    break;
                default:
                    if (guid != null)
                    {
                        Longpool.Instance.UpdateClient(state, guid);
                    }
                    break;

            }
        }
    }
}