using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using Wajeeh.Clinets;

namespace Wajeeh.Companies
{
    public class Company : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
    }
}
