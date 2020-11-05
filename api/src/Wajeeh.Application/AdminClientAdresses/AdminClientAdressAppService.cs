using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wajeeh.AdminClientAdresses.Dto;
using Wajeeh.ClientAdresses;

namespace Wajeeh.AdminClientAdresses
{
    public class AdminClientAdressAppService : AsyncCrudAppService<ClientAdress, AdminClientAdressDto, long, AdminPagedClientAdressResultRequestDto, AdminCreateClientAdressDto, AdminUpdateClientAdressDto>, IAdminClientAdressAppService
    {

        public AdminClientAdressAppService(IRepository<ClientAdress, long> repository) : base(repository)
        {
        }

        protected override IQueryable<ClientAdress> CreateFilteredQuery(AdminPagedClientAdressResultRequestDto input)
        {
            var query= base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.Keyword), t => t.Title.Contains(input.Keyword) || t.Adress.Contains(input.Keyword));

            query = query.Where(x => x.User.Client.Id == input.ClientId);
            return query;

        }
        protected override AdminClientAdressDto MapToEntityDto(ClientAdress entity)
        {
            var AdminClientAdressDto = base.MapToEntityDto(entity);



            return AdminClientAdressDto;
        }
    }
}
