using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Munchkin_Online.Core.Matchmaking;
using Munchkin_Online.Models;

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

        public Longpool()
        {
            Matchmaking.Matchmaking.Instance.MatchCreated += OnMatchCreated;
        }

        public void OnMatchCreated(object sender, MatchCreatedArgs e)
        {

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
        public void UpdateClient(ClientState state)
        {
            lock (_lock)
            {
                ClientState clientState = _clientStateList.Find(s => s.User == state.User);
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
            if (_clientStateList.Where(x => x.User.Id == state.User.Id).Count() != 0)
            {
                UpdateClient(state);
                return;
            }              
            lock (_lock)
            {
                state.ClientGuid = Guid.NewGuid().ToString("N");
                _clientStateList.Add(state);
            }
            Debug.WriteLine("User is here! Current users:{0}", _clientStateList.Count); 
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

        public object GetUserByGuid(string guid)
        {
            return Clients.Where(x => x.ClientGuid == guid).ToArray()[0];
        }


        public void PushMessageToUser(User User, AsyncMessage asyncMessage)
        {
            lock (_lock)
            {
                var clientState = _clientStateList.FirstOrDefault(x => x.User.Id == User.Id);
                if (clientState != null && clientState.IsCompleted == false)
                {
                    clientState.CurrentContext.Response.Write(asyncMessage.ToString());
                    clientState.CompleteRequest();
                }
            }
        }

        public void PushMessageToUser(Guid Guid, AsyncMessage asyncMessage)
        {
            lock (_lock)
            {
                var clientState = _clientStateList.FirstOrDefault(x => x.User.Id == Guid);
                if (clientState != null && clientState.IsCompleted == false)
                {
                    clientState.CurrentContext.Response.Write(asyncMessage.ToString());
                    clientState.CompleteRequest();
                }
            }
        }
    }
}