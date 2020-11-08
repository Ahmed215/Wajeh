using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Drivers.Dto;
using Wajeeh.Requests;
using Wajeeh.Reuests.Dto;
using Wajeeh.TrackingTrips;

namespace Wajeeh.TrackingTrips.Dto
{
    [AutoMap(typeof(TrackingTrip))]
    public class TrackingTripDto : EntityDto<long>
    {
       public long DriverId { get; set; }

        public DriverDto Driver { get; set; }
        public long RequestId { get; set; }
        public RequestDto Request { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    
}
