using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Wajeeh.RequestStatus.Dto
{
    [AutoMap(typeof(RequestState))]
    public class UpdateRequestStateDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
    }
}
