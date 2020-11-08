using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.Companies;
using Wajeeh.DriverNotifications;
using Wajeeh.TrackingTrips;

namespace Wajeeh.Drivers
{
    public class Driver : FullAuditedEntity<long>
    {
        public string VehicleSequenceNumber { get; set; }
        public string DriverIdentityNumber { get; set; }
        public bool IsDriverAvilable { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public long VehicleType { get; set; }
        //public string ProfilePicture { get; set; }
        //public string IdentityPicture { get; set; }
        //public string FrontVehiclePicture { get; set; }
        //public string BackVehiclePicture { get; set; }
        //public string LisencePicture { get; set; }
        //public string VehicleLisencePicture { get; set; }
    //
        public DateTime? DateOfBirthGregorian { get; set; }
        public string DateOfBirthHijri { get; set; }
        public string MobileNumber { get; set; }
       
        public int AddressTitle { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        //
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Company")]
        public long? CompanyId { get; set; }
        public Company Company { get; set; }
        public bool OffDuty { get; set; }
        //public ICollection<TrackingTrip> TrackingTrips { get; set; }
    }
}
