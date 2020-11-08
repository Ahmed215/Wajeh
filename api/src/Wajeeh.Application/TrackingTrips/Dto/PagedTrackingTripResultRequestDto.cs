using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.TrackingTrips.Dto
{
    public class PagedTrackingTripResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? TrackingTripId { get; set; }
    }
}
