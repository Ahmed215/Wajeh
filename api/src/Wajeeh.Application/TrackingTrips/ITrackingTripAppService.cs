using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories.Dto;
using Wajeeh.TrackingTrips.Dto;
using Wajeeh.TrackingTrips.Dto;

namespace Wajeeh.TrackingTrips
{
    public interface ITrackingTripAppService : IAsyncCrudAppService<TrackingTripDto, long, PagedTrackingTripResultRequestDto, CreateTrackingTripDto, UpdateTrackingTripDto>
    {
        List<TrackingTripDto> GetTrackingTrip(GetTrackingTrip input);
    }
}
