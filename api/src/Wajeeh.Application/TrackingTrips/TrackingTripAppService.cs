using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Categories.Dto;
using Abp.Linq.Extensions;
using System.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Abp.Extensions;
using Wajeeh.TrackingTrips.Dto;
using System.Security.Cryptography.X509Certificates;
using Abp.UI;
using Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;

namespace Wajeeh.TrackingTrips
{
    public class TrackingTripAppService : AsyncCrudAppService<TrackingTrip, TrackingTripDto, long, PagedTrackingTripResultRequestDto, CreateTrackingTripDto, UpdateTrackingTripDto>, ITrackingTripAppService
    {

        private readonly IObjectMapper _objectMapper;

        public TrackingTripAppService(IRepository<TrackingTrip, long> repository, IObjectMapper objectMapper) : base(repository)
        {
            _objectMapper = objectMapper;
        }

        protected override IQueryable<TrackingTrip> CreateFilteredQuery(PagedTrackingTripResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            query = query.WhereIf(input.TrackingTripId.HasValue && input.TrackingTripId.Value > 0, t => t.Id == input.TrackingTripId);
            if (!input.Keyword.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.Keyword);
                    string Id = filter_query["Id"];
                    query = query.WhereIf(!Id.IsNullOrEmpty(), t => t.Id== float.Parse(Id));

                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException("there is exception");
                }
            }
            return query;
        }
      
        protected override TrackingTripDto MapToEntityDto(TrackingTrip entity)
        {
            var TrackingTripDto = base.MapToEntityDto(entity);


            return TrackingTripDto;
        }
        public List<TrackingTripDto> GetTrackingTrip(GetTrackingTrip input) {
            try
            {
                var tt = Repository.GetAll().Where(x => x.RequestId == input.RequestId && x.DriverId == input.DriverId).Include(x=>x.Request.UserRequset.Driver).ToList();
                if (tt.Count>0)
                {
                    return _objectMapper.Map<List<TrackingTripDto>>(tt);
                }
                else
                {
                    throw new UserFriendlyException("there is no trip");
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException("there is no trip");


            }
           
            
           


        }
    }
}
