using Abp.AutoMapper;
using Wajeeh.Clinets;

namespace Wajeeh.AdminClients.Dto
{
    [AutoMap(typeof(Client))]
    public class AdminCreateClientDto
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public long? CompanyId { get; set; }
    }
}
