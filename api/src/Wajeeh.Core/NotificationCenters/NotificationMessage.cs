using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.NotificationCenters
{
    public class NotificationMessage
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; }
        public object data { get; set; }
    }
}
