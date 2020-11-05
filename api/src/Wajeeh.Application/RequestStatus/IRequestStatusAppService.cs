using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.RequestStatus.Dto;

namespace Wajeeh.RequestStatus
{
    public interface IRequestStatusAppService
    {
        List<RequestStateDto> GetAllRequestStatus();
    }
}
