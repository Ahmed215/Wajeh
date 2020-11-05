using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminWaselDrivers.Dto;
using Wajeeh.Wasel;
using Wajeeh.WaselDrivers;
using System.Linq;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Wajeeh.Authorization;
using Abp.Authorization;
using Abp.Extensions;
using Wajeeh.Wasel.models;

namespace Wajeeh.AdminWaselDrivers
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminWaselDriverAppService : AsyncCrudAppService<WaselDriver, AdminWaselDriverDto, long, AdminPagedWaselDriverResultRequestDto, AdminCreateWaselDriverDto, AdminUpdateWaselDriverDto>, IAdminWaselDriverAppService
    {
        private readonly IWaselService _waselService;
        public AdminWaselDriverAppService(IRepository<WaselDriver, long> repository,
            IWaselService waselService) : base(repository)
        {
            _waselService = waselService;
        }

        protected override IQueryable<WaselDriver> CreateFilteredQuery(AdminPagedWaselDriverResultRequestDto input)
        {
            var query = base.CreateFilteredQuery(input);
            if (!input.KeyWord.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.KeyWord);
                    string email = filter_query["email"];
                    query = query.WhereIf(!email.IsNullOrEmpty(), t => t.email.Contains(email));
                    string dateOfBirthGregorian = filter_query["dateOfBirthGregorian"];
                    query = query.WhereIf(!dateOfBirthGregorian.IsNullOrEmpty(), t => t.dateOfBirthGregorian.Contains(dateOfBirthGregorian));
                    string mobileNumber = filter_query["mobileNumber"];
                    query = query.WhereIf(!mobileNumber.IsNullOrEmpty(), t => t.mobileNumber.Contains(mobileNumber));
                    string identityNumber = filter_query["identityNumber"];
                    query = query.WhereIf(!identityNumber.IsNullOrEmpty(), t => t.identityNumber.Contains(identityNumber));
                }
                catch
                {
                    query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), t => t.email.Contains(input.KeyWord) || t.identityNumber.Contains(input.KeyWord)
                                                                 || t.mobileNumber.Contains(input.KeyWord) || t.dateOfBirthGregorian.Contains(input.KeyWord));
                }
            }
            return query;
        }

        //public override async Task<AdminWaselDriverDto> CreateAsync(AdminCreateWaselDriverDto input)
        //{
        //    var isreged = _waselService.RegDriver(new Wasel.models.RegDriverVM());
        //    {
        //        dateOfBirthGregorian = input.dateOfBirthGregorian,
        //        dateOfBirthHijri = input.dateOfBirthHijri,
        //        email = input.email,
        //        identityNumber = input.identityNumber,
        //        mobileNumber = input.mobileNumber
        //    });
        //    if (!string.IsNullOrEmpty(isreged))
        //    {
        //        var result = await base.CreateAsync(input);
        //        await CurrentUnitOfWork.SaveChangesAsync();
        //        return result;
        //    }
        //    return null;
        //}

        
    }
}
