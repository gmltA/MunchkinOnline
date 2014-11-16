using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Core.Database;
using Munchkin_Online.Core.Longpool;
using Munchkin_Online.Models;
using Ninject;

namespace Munchkin_Online.Core
{
    /// <summary>
    /// Класс, характеризующий подключенный клиент
    /// </summary>
    public class ClientState : IAsyncResult
    {
        public HttpContext CurrentContext { get; set; }
        public AsyncCallback AsyncCallback { get; set; }
        public User User { get; set; }
        public object ExtraData { get; set; }
        public string ClientGuid { get; set; }

        HashSet<AsyncMessage> Messages = new HashSet<AsyncMessage>();

        private Boolean _isCompleted;

        public ClientState(HttpContext context, AsyncCallback callback, object data)
        {
            CurrentContext = context;
            AsyncCallback = callback;
            ExtraData = data;
            _isCompleted = false;
            User = ((UserIndentity)context.User.Identity).User;
        }

        public void SetUncomplete()
        {
            _isCompleted = false;
        }

        public bool AddToPool(AsyncMessage message)
        {
            return Messages.Add(message); 
        }

        public void Push(AsyncMessage message)
        {
            if (IsCompleted)
            {
                Messages.Add(message);
            }
            else
            {
                CurrentContext.Response.Write(message.ToString());
                CompleteRequest();
            }
        }

        public void CompleteMessages()
        {
            var message = Messages.ElementAtOrDefault(0);
            if (message != null)
            {
                Push(message);
                Messages.Remove(message);
            }
        }

        public void CompleteRequest()
        {
            _isCompleted = true;
            if (AsyncCallback != null)
            {
                AsyncCallback(this);
            }
        }

        #region IAsyncResult Members

        public Boolean CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }

        public object AsyncState
        {
            get
            {
                return ExtraData;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return new ManualResetEvent(false);
            }
        }
        #endregion
    }  

}
