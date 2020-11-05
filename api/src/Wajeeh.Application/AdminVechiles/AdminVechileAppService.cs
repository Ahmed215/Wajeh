using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.AdminVechiles.Dto;
using Wajeeh.Authorization;
using Wajeeh.Vechiles;
using System.Linq;
using Abp.Linq.Extensions;
using Wajeeh.Wasel;
using System.Threading.Tasks;
using Abp.Extensions;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Wajeeh.PlateTypes;

namespace Wajeeh.AdminVechiles
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class AdminVechileAppService : AsyncCrudAppService<Vechile, AdminVechileDto, long, AdminPagedVechcileResultRequestDto, CreateAdminVechileDto, UpdateAdminVechileDto>, IAdminVechileAppService
    {
        private readonly IWaselService _waselService;
        private readonly IRepository<PlateType, long> _plateTypeRepository;
        public AdminVechileAppService(IRepository<Vechile, long> repository,
            IWaselService waselService, IRepository<PlateType, long> plateTypeRepository) : base(repository)
        {
            _waselService = waselService;
            _plateTypeRepository = plateTypeRepository;
        }
        protected override IQueryable<Vechile> CreateFilteredQuery(AdminPagedVechcileResultRequestDto input)
        {

            var query = base.CreateFilteredQuery(input);
            if (!input.KeyWord.IsNullOrEmpty())
            {
                try
                {
                    dynamic filter_query = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input.KeyWord);
                    string Plate = filter_query["Plate"];
                    query = query.WhereIf(!Plate.IsNullOrEmpty(), t => t.Plate.Contains(Plate));
                    string SequenceNumber = filter_query["SequenceNumber"];
                    query = query.WhereIf(!SequenceNumber.IsNullOrEmpty(), t => t.SequenceNumber.Contains(SequenceNumber));

                    string PlateTypeName = filter_query["PlateTypeName"];
                    if (!PlateTypeName.IsNullOrEmpty())
                    {
                        var plateTypeIds = _plateTypeRepository.GetAll()
                            .Where(x =>
                            (CultureInfo.CurrentCulture.Name != "ar-EG" && x.Name.Contains(PlateTypeName)) ||
                            (CultureInfo.CurrentCulture.Name == "ar-EG" && x.NameAr.Contains(PlateTypeName)))
                            .Select(x => x.Id).ToArray();
                        query = query.WhereIf(plateTypeIds != null, t => plateTypeIds.Contains(t.PlateType));
                    }
                }
                catch
                {
                    query = query.WhereIf(!string.IsNullOrEmpty(input.KeyWord), t => t.Plate.Contains(input.KeyWord) || t.SequenceNumber.Contains(input.KeyWord));
                }
            }
            return query;
        }

        //[AbpAllowAnonymous]
        public async Task<string> CreateVechileAsync(CreateAdminVechileDto input)
        {
            var waselVechile = _waselService.RegVechile(new Wasel.models.VechileRegVm()
            {
                Plate = input.Plate,
                PlateType = input.PlateType,
                SequenceNumber = input.SequenceNumber
            });
            if (waselVechile.success)
            {
                try
                {
                    var vechile = new Vechile()
                    {
                        Plate = input.Plate,
                        PlateType = input.PlateType,
                        SequenceNumber = input.SequenceNumber
                    };
                    await Repository.InsertAsync(vechile);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }

            }
            return waselVechile.resultCode;
        }

        protected override AdminVechileDto MapToEntityDto(Vechile entity)
        {
            var adminVechileDto = base.MapToEntityDto(entity);
            var plateType = _plateTypeRepository.Get(entity.PlateType);
            if (CultureInfo.CurrentCulture.Name == "ar-EG")
            {

                adminVechileDto.PlateTypeName = plateType.NameAr;
            }
            else
            {
                adminVechileDto.PlateTypeName = plateType.Name;
            }
            return adminVechileDto;
        }
    }
}
