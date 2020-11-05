using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Wasel.models
{
    public class ApiResultVm
    {
        public string logId { get; set; }
        public string content { get; set; }
        public ApiResultStatusVm status { get; set; }
    }
}
