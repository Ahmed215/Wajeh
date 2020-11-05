using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Drivers.Dto;

namespace Wajeeh.Drivers
{
    public interface IDriverAppService : IAsyncCrudAppService<DriverDto, long, PagedDriverResultRequestDto, CreateDriverDto, UpdateDriverDto>
    {
        public bool IsUserHaseProfile(long userId);
        public DriverDto CreateProfile(CreateDriverDto model);
        public DriverDto UpdateProfile(UpdateDriverDto model);
        public DriverDto GetProfile();
        public DriverDto GetByUserId(long userId);
        public bool SetDriverOffDuty(long userId, bool offDuty);
    }
}
