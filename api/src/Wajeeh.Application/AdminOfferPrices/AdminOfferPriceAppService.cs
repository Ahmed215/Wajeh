using Abp.Application.Services;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wajeeh.AdminOfferPrices.Dto;
using Wajeeh.Authorization;
using Wajeeh.Drivers;
using Wajeeh.OfferPrices;

namespace Wajeeh.AdminOfferPrices
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminOfferPriceAppService : AsyncCrudAppService<OfferPrice, AdminOfferPriceDto, long, AdminPagedOfferPriceResultRequestDto, AdminCreateOfferPriceDto,
        AdminUpdateOfferPriceDto>, IAdminOfferPriceAppService
    {
        private readonly IRepository<Driver, long> _driverRepository;

        public AdminOfferPriceAppService(IRepository<OfferPrice, long> repository, IRepository<Driver, long> driverRepository) : base(repository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<List<TopRequestSalesDriverDto>> GetTopRequestSalesDrivers(GetTopRequestSalesDriverInput input)
        {
            var result = await Repository.GetAll()
                .Where(x => x.IsAccepted.HasValue && x.IsAccepted.Value == true)
                .GroupBy(p => new { p.DriverId, p.DriverName })
                .Select(g => new TopRequestSalesDriverDto()
                {
                    Id = g.Key.DriverId,
                    DriverName = g.Key.DriverName,
                    RequestsCount = g.Count()
                })
                .OrderByDescending(x=>x.RequestsCount)
                .ToListAsync();


            if (result != null)
            {
                foreach (var item in result)
                {
                    try
                    {
                        var driver = await _driverRepository.GetAllIncluding(x => x.User).FirstOrDefaultAsync(x => x.UserId == item.Id);
                        if (driver == null)
                            continue;

                        item.Email = driver.Email;
                        item.DriverName = driver.FullName;
                        item.Id = driver.Id;
                        item.Phone = driver.User.UserName;
                    }
                    catch
                    {
                    }
                }
                result = result
                    .WhereIf(!input.DriverName.IsNullOrEmpty(), x => x.DriverName.Contains(input.DriverName))
                    .WhereIf(!input.Phone.IsNullOrEmpty(), x => x.Phone.Contains(input.Phone))
                    .WhereIf(!input.Email.IsNullOrEmpty(), x => x.Email.Contains(input.Email))
                    .ToList();
            }
            return result;
        }
    }
}
