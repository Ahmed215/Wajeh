using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.DriverJoinRequests;

namespace Wajeeh.AdminDriverJoinRequests.Dto
{
    [AutoMap(typeof(DriverJoinRequest))]
    public class AdminCreateDriverJoinRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhone { get; set; }
        public string CompanyName { get; set; }
    }
}
