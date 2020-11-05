using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Wajeeh.DriverJoinRequests;

namespace Wajeeh.AdminDriverJoinRequests.Dto
{
    [AutoMap(typeof(DriverJoinRequest))]
    public class DriverJoinRequestDto : EntityDto<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhone { get; set; }
        public string CompanyName { get; set; }
    }
}
