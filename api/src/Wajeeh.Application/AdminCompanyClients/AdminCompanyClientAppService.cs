using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wajeeh.AdminCompanyClients.Dto;
using Wajeeh.Authorization;
using Wajeeh.Authorization.Users;
using Wajeeh.CompanyClients;

namespace Wajeeh.AdminCompanyClients
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminCompanyClientAppService : AsyncCrudAppService<CompanyClient, AdminCompanyClientDto, long, AdminPagedCompanyClientResultRequestDto, AdminCreateCompanyClientDto, AdminUpdateCompanyClientDto>, IAdminCompanyClientAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CompanyClient, long> _repository;


        public AdminCompanyClientAppService(IRepository<CompanyClient, long> repository, IRepository<User, long> userRepository) : base(repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }


        protected override IQueryable<CompanyClient> CreateFilteredQuery(AdminPagedCompanyClientResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input)
            .WhereIf(!string.IsNullOrEmpty(input.Keyword), t => t.FullName.Contains(input.Keyword) 
            || t.Email.Contains(input.Keyword) || t.User.UserName.Contains(input.Keyword));

            if (input.CompanyId != null)
                query = query.Where(x => x.CompanyId == input.CompanyId);

            

            return query;
        }

        protected override IQueryable<CompanyClient> ApplySorting(IQueryable<CompanyClient> query, AdminPagedCompanyClientResultRequestDto input)
        {
            if (input.Sorting != null)
            {
                if (input.Sorting.StartsWith("Phone"))
                {
                    if (input.Sorting.EndsWith("Asc"))
                    {
                        return query.OrderBy(p => p.User.UserName);

                    }
                    else
                    {
                        return query.OrderByDescending(p => p.User.UserName);

                    }
                }

                else
                {
                    return base.ApplySorting(query, input);
                }
            }

            else
            {
                return base.ApplySorting(query, input);
            }
        }


        public override Task<AdminCompanyClientDto> UpdateAsync(AdminUpdateCompanyClientDto input)
        {
            var userid = _repository.GetAll().Where(x => x.Id == input.Id).Select(x => x.UserId).FirstOrDefault();
            var userEntity = _userRepository.GetAll().Where(x => x.Id == userid).FirstOrDefault();

            if (userEntity.UserName != input.Phone)
            {
                if (_userRepository.GetAll().Any(x => x.UserName == input.Phone))
                    throw new Exception();
            }
            return base.UpdateAsync(input);
        }
        protected override AdminCompanyClientDto MapToEntityDto(CompanyClient entity)
        {
            var companyClient = base.MapToEntityDto(entity);
            companyClient.Phone = _userRepository.FirstOrDefault(x => x.Id == entity.UserId) !=null? _userRepository.FirstOrDefault(x => x.Id == entity.UserId).UserName:string.Empty;
            return companyClient;
          
        }
    }
}
