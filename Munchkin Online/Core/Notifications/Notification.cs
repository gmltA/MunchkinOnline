using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Munchkin_Online.Core.Notifications
{
    public class Notification
    {
        public string Message { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime Posted { get; private set; }

        public Notification(string notificationMessage, NotificationType notificationType)
        {
            Message = notificationMessage;
            Type = notificationType;
            Posted = DateTime.Now;
        }
    }

    public enum NotificationType
    {
            Normal,
            Warning,
            Error
    }
}