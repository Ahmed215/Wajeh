using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Users.Dto
{
    public class AdminMessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime MessageTime { get; set; }
        public bool IsFromAdmin { get; set; }
    }
}
