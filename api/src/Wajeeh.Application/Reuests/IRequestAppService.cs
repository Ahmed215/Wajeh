using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Reuests.Dto;

namespace Wajeeh.Reuests
{
    public interface IRequestAppService : IAsyncCrudAppService<RequestDto, long, PagedRequestResultRequestDto, CreateRequestDto, UpdateRquestDto>
    {
        RequestDto CreateRequest(CreateRequestDto model);
    }
}
