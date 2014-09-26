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

        public Notification(string notificationMessage, NotificationType notificationType)
        {
            Message = notificationMessage;
            Type = notificationType;
        }
    }

    public class TimedList<T> : List<T>
    {
        public DateTime LastUpdate { get; private set; }

        public TimedList()
            : base()
        {
            UpdateTimeout();
        }

        public TimedList(int capacity)
            : base(capacity)
        {
            UpdateTimeout();
        }

        public TimedList(IEnumerable<T> collection)
            : base(collection)
        {
            UpdateTimeout();
        }

        new public void Add(T item)
        {
            UpdateTimeout();
            base.Add(item);
        }

        new public void Clear()
        {
            UpdateTimeout();
            base.Clear();
        }

        private void UpdateTimeout()
        {
            LastUpdate = DateTime.Now;
        }
    }

    public enum NotificationType
    {
            Normal,
            Warning,
            Error
    }
}