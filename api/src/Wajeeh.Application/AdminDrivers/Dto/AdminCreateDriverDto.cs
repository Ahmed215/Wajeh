using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Drivers;

namespace Wajeeh.AdminDrivers.Dto
{
    [AutoMap(typeof(Driver))]
    public class AdminCreateDriverDto
    {
        public string VehicleSequenceNumber { get; set; }
        public string DriverIdentityNumber { get; set; }
        public bool IsDriverAvilable { get; set; }
        public bool OffDuty { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public long VehicleType { get; set; }
        public string ProfilePicture { get; set; }
        public string IdentityPicture { get; set; }
        public string FrontVehiclePicture { get; set; }
        public string BackVehiclePicture { get; set; }
        public string LisencePicture { get; set; }
        public string VehicleLisencePicture { get; set; }
        public long CompanyId { get; set; }
    }
}
