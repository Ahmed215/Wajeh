using System;
using System.Collections.Generic;
using Wajeeh.PlateTypes.Dto;

namespace Wajeeh.PlateTypes
{
    public interface IPlateTypesAppService
    {
        List<PlateTypeDto> GetAllPlateTypes();
    }
}
