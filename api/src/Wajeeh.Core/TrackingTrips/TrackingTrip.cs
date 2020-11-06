using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Drivers;
using Wajeeh.Requests;

namespace Wajeeh.TrackingTrips
{
   public  class TrackingTrip:FullAuditedEntity<long>
    {
        public long DriverId { get; set; }
        [ForeignKey("DriverId")]
        public Driver Driver { get; set; }
        public long RequestId { get; set; }
        [ForeignKey("RequestId")]
        public Request Request { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
