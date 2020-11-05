using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Wasel.models;

namespace Wajeeh.Wasel
{
    public interface IWaselService: IApplicationService
    {
        string RegTrip(TripRegVM viewModel);
        WaselResponseDTOC RegVechile(VechileRegVm viewModel);
        bool UpdateTrip(TripUpdateVM viewModel);
        WaselResponseDTOC RegDriver(RegDriverVM viewModel);
      
    }
}
