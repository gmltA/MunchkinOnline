using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munchkin_Online.Core.Longpool
{
    public class Longpool
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static readonly Longpool Instance = new Longpool();

        private Object _lock = new Object();

        /// <summary>
        /// Clients.
        /// </summary>
        private List<ClientState> _clientStateList =
          new List<ClientState>();

        public List<ClientState> Clients
        {
            get
            {
                return _clientStateList;
            }
        }

        /// <summary>
        /// Send message to all clients
        /// </summary>
        /// <param name="message"></param>
        public void PushMessage(AsyncMessage message)
        {
            lock (_lock)
            {
                foreach (ClientState clientState in _clientStateList)
                {
                    if (clientState.CurrentContext.Session != null)
                    {
                        clientState.CurrentContext.Response.Write(message.ToString());
                        clientState.CompleteRequest();
                    }
                }
            }
        }

        /// <summary>
        /// Обновление клиента при таймауте
        /// </summary>
        /// <param name="state"></param>
        /// <param name="guid"></param>
        public void UpdateClient(ClientState state, String guid)
        {
            lock (_lock)
            {
                ClientState clientState = _clientStateList.Find(s => s.ClientGuid
                  == guid);
                if (clientState != null)
                { 
                    clientState.CurrentContext = state.CurrentContext;
                    clientState.ExtraData = state.ExtraData;
                    clientState.AsyncCallback = state.AsyncCallback;
                }
            }
        }

        /// <summary>
        /// Новый клиент.
        /// </summary>
        /// <param name="state">Состояние</param>
        public void RegicterClient(ClientState state)
        {
            lock (_lock)
            {
                state.ClientGuid = Guid.NewGuid().ToString("N");
                _clientStateList.Add(state);
            }
        }

        /// <summary>
        /// Что бы это могло быть?
        /// </summary>
        /// <param name="state">Какой-то параметр</param>
        public void UnregisterClient(ClientState state)
        {
            lock (_lock)
            {
                _clientStateList.Remove(state);
            }
        }
    }
}