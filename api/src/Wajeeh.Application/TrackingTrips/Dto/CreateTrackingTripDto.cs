using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wajeeh.TrackingTrips;

namespace Wajeeh.TrackingTrips.Dto
{
    [AutoMap(typeof(TrackingTrip))]
    public class CreateTrackingTripDto
    {
        public long DriverId { get; set; }
   
        [Required]
        public long RequestId { get; set; }
  
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
