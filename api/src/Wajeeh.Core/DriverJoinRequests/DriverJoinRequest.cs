using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Wajeeh.DriverJoinRequests
{
    public class DriverJoinRequest : FullAuditedEntity<long>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string MobilePhone { get; set; }
        public string CompanyName { get; set; }

    }
}
