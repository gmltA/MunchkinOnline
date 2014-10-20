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
                    clientState.Push(message);
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
                    clientState.SetUncomplete();
                    clientState.CurrentContext = state.CurrentContext;
                    clientState.ExtraData = state.ExtraData;
                    clientState.AsyncCallback = state.AsyncCallback;
                    clientState.CompleteMessages();
                }
            }
        }

        /// <summary>
        /// Новый клиент.
        /// </summary>
        /// <param name="state">Состояние</param>
        public void RegisterClient(ClientState state)
        {
            var st = _clientStateList.FirstOrDefault(x => x.User.Id == state.User.Id);
            if (st != null)
            {
                //st.Push(new AsyncMessage(MessageType.StopPooling));
                UpdateClient(state);
                return;
            }              
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

        public object GetUserByGuid(string guid)
        {
            return Clients.Where(x => x.ClientGuid == guid).ToArray()[0];
        }


        public void PushMessageToUser(User User, AsyncMessage asyncMessage)
        {
            lock (_lock)
            {
                var clientState = _clientStateList.FirstOrDefault(x => x.User.Id == User.Id);
                if (clientState != null)
                {
                    clientState.Push(asyncMessage);
                }
            }
        }

        public void PushMessageToUser(Guid Guid, AsyncMessage asyncMessage)
        {
            lock (_lock)
            {
                var clientState = _clientStateList.FirstOrDefault(x => x.User.Id == Guid);
                if (clientState != null)
                {
                    clientState.Push(asyncMessage);
                }
            }
        }
    }
}