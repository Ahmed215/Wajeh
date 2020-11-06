using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.Companies;
using Wajeeh.DriverNotifications;

namespace Wajeeh.TrackingTrips
{
    public class TrackingTrip : FullAuditedEntity<long>
    {
        public long DriverId { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
      

    }
}
