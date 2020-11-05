using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Wajeeh.RequestStatus.Dto;

namespace Wajeeh.RequestStatus
{
    public class RequestStatusAppService : AsyncCrudAppService<RequestState, RequestStateDto, long, PagedRequestStateDto, CreateRequestStateDto, UpdateRequestStateDto>, IRequestStatusAppService
    {
        private readonly IRepository<RequestState, long> repository;
        private readonly IHttpContextAccessor httpContext;

        public RequestStatusAppService(IRepository<RequestState, long> repository, IHttpContextAccessor httpContext) : base(repository)
        {
            this.repository = repository;
            this.httpContext = httpContext;
        }
        //as enum
        IDictionary<int, string> RquestStatus = new Dictionary<int, string>() {
                 
                    { 2, "Scheduled , مجدول"},
                    { 1, "Current , الحالي"},
                    { 3, "Previous , السابق"},
                    { 4, "Cancelled , ملغي"},
                };
        public List<RequestStateDto> GetAllRequestStatus()
        {
            var lang = "ar-EG";
            var Headers = httpContext.HttpContext.Request.Headers;
            if (Headers.ContainsKey("Content-Language"))
            {
                lang = Headers["Content-Language"].ToString();
            }
            var statusList = new List<RequestStateDto>();
            foreach (var status in RquestStatus)
            {
                statusList.Add(new RequestStateDto
                {
                    Id = status.Key,
                    Name = lang == "ar-EG" ? status.Value.Split(',')[1] : status.Value.Split(',')[0],
                    NameAr = status.Value.Split(',')[1],
                    NameEn = status.Value.Split(',')[0]
                }
                );

            }
   
            return statusList;


            //var requestStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.Name }).ToList();
            //var requestStatusDto = new List<RequestStateDto>();
            //var Headers = httpContext.HttpContext.Request.Headers;
            //if (Headers.ContainsKey("Content-Language"))
            //{
            //    var lang = Headers["Content-Language"].ToString();
            //    if (lang.ToLower() == "ar-EG")
            //    {
            //        requestStatus = repository.GetAll().Select(q => new { Id = q.Id, Name = q.NameAr }).ToList();
            //    }
            //}
            //requestStatusDto = requestStatus.Select(s => MapToEntityDto(new RequestState { Id = s.Id, Name = s.Name })).ToList();
            //return requestStatusDto;
        }
    }
}
