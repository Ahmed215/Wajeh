using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Clients.Dto;

namespace Wajeeh.Clients
{
    public interface IClientAppService : IAsyncCrudAppService<ClientDto, long, PagedClientResultRequestDto, CreateClientDto, UpdateClientDto>
    {
        public bool IsUserHaseProfile(long userId);
        public ClientDto CreateProfile(CreateClientDto model);
        public ClientDto GetProfile();
        public ClientDto UpdateProfile(UpdateClientDto model);
        public ClientDto GetByUserId(long userId);

    }
}
