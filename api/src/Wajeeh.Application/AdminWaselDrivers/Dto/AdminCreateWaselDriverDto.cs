using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.WaselDrivers;

namespace Wajeeh.AdminWaselDrivers.Dto
{
    [AutoMap(typeof(WaselDriver))]
    public class AdminCreateWaselDriverDto
    {
        public string identityNumber { get; set; }
        public string dateOfBirthGregorian { get; set; }
        public string dateOfBirthHijri { get; set; }
        public string mobileNumber { get; set; }
        public string email { get; set; }
    }
}
