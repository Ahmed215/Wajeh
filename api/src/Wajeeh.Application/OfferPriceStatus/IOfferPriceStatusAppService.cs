using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.OfferPriceStatus.Dto;
using Wajeeh.RequestStatus.Dto;

namespace Wajeeh.OfferPriceStatus
{
    public interface IOfferPriceStatusAppService
    {
        List<OfferPriceStatusDto> GetAllOfferPriceStatus();
    }
}
