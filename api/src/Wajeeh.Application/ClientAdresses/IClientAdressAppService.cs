using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.ClientAdresses.Dto;

namespace Wajeeh.ClientAdresses
{
    public interface IClientAdressAppService : IAsyncCrudAppService<ClientAdressDto, long, PagedClientAdressResultRequestDto, CreateClientAdressDto, UpdateClientAdressDto>
    {
        List<ClientAdressDto> GetClientDress();
    }
}
