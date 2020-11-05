using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.Companies;

namespace Wajeeh.Clinets
{
    public class Client : FullAuditedEntity<long>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Company")]
        public long? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
