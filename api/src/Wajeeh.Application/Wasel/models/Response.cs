using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Wasel.models
{
    public class Response<T> where T : class
    {
        public DateTime? RequestDateTime { get; set; }
        public string LogId { get; set; }
        public Status Status { get; set; }
        public T Content { get; set; }
    }
}
