using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Munchkin_Online.Core.Auth;
using Munchkin_Online.Models;
using Ninject;

namespace Munchkin_Online.Core.Longpool
{
    /// <summary>
    /// Класс, характеризующий подключенный клиент/
    /// </summary>
    public class ClientState : IAsyncResult
    {
        /*[Inject]
        IAuthentication Auth { get; set; }

        User CurrentUser
        {
            get
            {
                return ((UserIndentity)(Auth.CurrentUser.Identity)).User;
            }
        }*/

        public HttpContext CurrentContext { get; set; }
        public AsyncCallback AsyncCallback { get; set; }
        public User User { get; set; }
        public object ExtraData { get; set; }
        public string ClientGuid { get; set; }

        private Boolean _isCompleted;

        public ClientState(HttpContext context,
          AsyncCallback callback, object data)
        {
            CurrentContext = context;
            AsyncCallback = callback;
            ExtraData = data;
            _isCompleted = false;
            //User = CurrentUser;
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
