using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Users.Dto
{
    public class MessageDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
