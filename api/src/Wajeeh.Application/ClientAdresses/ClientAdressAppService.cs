using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using Wajeeh.ClientAdresses.Dto;
using System.Linq;
using Abp.Runtime.Session;
using System.Threading.Tasks;

namespace Wajeeh.ClientAdresses
{
    public class ClientAdressAppService : AsyncCrudAppService<ClientAdress, ClientAdressDto, long, PagedClientAdressResultRequestDto, CreateClientAdressDto, UpdateClientAdressDto>, IClientAdressAppService
    {
        private readonly IRepository<ClientAdress, long> _repository;
        public ClientAdressAppService(IRepository<ClientAdress, long> repository) : base(repository)
        {
            _repository = repository;
        }

        public List<ClientAdressDto> GetClientDress()
        {
            var userId = AbpSession.GetUserId();
            var adresses = _repository.GetAll().Where(a => a.UserId == userId);
            return adresses.Select(a => ObjectMapper.Map<ClientAdressDto>(a)).ToList();
        }



        public ClientAdressDto UpdateClientAddress(UpdateClientAdressDto input)
        {
            var userId = AbpSession.GetUserId();
            var address = _repository.GetAll().Where(a => a.UserId == userId && a.Id == input.Id).FirstOrDefault();
            if (address == null)
                throw new Exception("not found");

            MapToEntity(input, address);
            return MapToEntityDto(address);
        }
    }
}
