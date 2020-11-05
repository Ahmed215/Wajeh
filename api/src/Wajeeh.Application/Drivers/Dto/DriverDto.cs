using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Drivers.Dto
{
    [AutoMap(typeof(Driver))]
    public class DriverDto : EntityDto<long>
    {
        public bool IsDriverAvilable { get; set; }
        public bool OffDuty { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public long VehicleType { get; set; }
        public string ProfilePicture { get; set; }
        public string IdentityPicture { get; set; }
        public string FrontVehiclePicture { get; set; }
        public string BackVehiclePicture { get; set; }
        public string LisencePicture { get; set; }
        public string VehicleLisencePicture { get; set; }
        public int NotificationsCount { get; set; }
    }
}
