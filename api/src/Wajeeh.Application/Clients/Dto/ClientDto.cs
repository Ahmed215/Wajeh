using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Clinets;

namespace Wajeeh.Clients.Dto
{
    [AutoMap(typeof(Client))]
    public class ClientDto : EntityDto<long>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public int NotificationsCount { get; set; }
        public long? CompanyId { get; set; }
    }
}
