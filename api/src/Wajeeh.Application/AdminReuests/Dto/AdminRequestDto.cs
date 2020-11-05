using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.OfferPrices.Dto;
using Wajeeh.Requests;

namespace Wajeeh.AdminReuests.Dto
{
    [AutoMap(typeof(Request))]
    public class AdminRequestDto : EntityDto<long>, IHasCreationTime
    {
        public bool? IsReg { get; set; }
        public double DiscountPercentage { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public int PaymentWay { get; set; }
        public string Notes { get; set; }
        public string StartingPoint { get; set; }
        public string EndingPoint { get; set; }
        public string StratingPointAdress { get; set; }
        public string StratingPointTitle { get; set; }
        public string EndingPointAdress { get; set; }
        public string EndingPointTitle { get; set; }
        public long SubcategoryId { get; set; }
        public string SubcategoryDisplayName { get; set; }
        public int Status { get; set; }
        public string RequestStateName { get; set; }
        public string DeliveryCost { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
        public DateTime CreationTime { get; set; }
        public string CTString { get; set; }
        //public OfferPriceDto AcceptedOfferPrice { get; set; }
        public string AcceptedDriverName { get; set; }
        public double Net { get; set; }
        public string CustomerName { get; set; }
        public double VAT { get; set; }
        public double VATAmount { get; set; }
    }


    public class TopRequestSalesClientDto : EntityDto<long>
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RequestsCount { get; set; }
    }

    public class GetTopRequestSalesClientInput 
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

   
}
