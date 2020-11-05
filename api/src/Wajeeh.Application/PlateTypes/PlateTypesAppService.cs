using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Wajeeh.PlateTypes.Dto;

namespace Wajeeh.PlateTypes
{
    public class PlateTypesAppService : AsyncCrudAppService<PlateType, PlateTypeDto, long, PagedPlateTypeDto, CreatePlateTypeDto, UpdatePlateTypeDto>, IPlateTypesAppService
    {
        private readonly IRepository<PlateType, long> repository;
        private readonly IHttpContextAccessor httpContext;

        public PlateTypesAppService(IRepository<PlateType, long> repository, IHttpContextAccessor httpContext) : base(repository)
        {
            this.repository = repository;
            this.httpContext = httpContext;
        }

        public List<PlateTypeDto> GetAllPlateTypes()
        {
            var requestStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.Name }).ToList();
            var requestStatusDto = new List<PlateTypeDto>();
            var Headers = httpContext.HttpContext.Request.Headers;
            if (Headers.ContainsKey("Content-Language"))
            {
                var lang = Headers["Content-Language"].ToString();
                if (lang.ToLower() == "ar-EG")
                {
                    requestStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.NameAr }).ToList();
                }
            }
            requestStatusDto = requestStatus.Select(s => MapToEntityDto(new PlateType { Id = s.Id, Name = s.Name })).ToList();
            return requestStatusDto;
        }
    }
}
