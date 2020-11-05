using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;

namespace Wajeeh.ClientAdresses
{
    public class ClientAdress : FullAuditedEntity<long>
    {
        public string Adress { get; set; }
        public string Title { get; set; }
        public string LangLat { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
