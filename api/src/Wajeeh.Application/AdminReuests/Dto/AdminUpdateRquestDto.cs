using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Requests;

namespace Wajeeh.AdminReuests.Dto
{
    [AutoMap(typeof(Request))]
    public class AdminUpdateRquestDto : EntityDto<long>
    {
        public bool? IsReg { get; set; }
        public double DiscountPercentage { get; set; }

        public DateTime ArrivalDateTime { get; set; }

        public int PaymentWay { get; set; }

        public string Notes { get; set; }

        public string StartingPoint { get; set; }
        public string StratingPointAdress { get; set; }
        public string StratingPointTitle { get; set; }
        public string EndingPointAdress { get; set; }
        public string EndingPointTitle { get; set; }

        public string EndingPoint { get; set; }
        public long SubcategoryId { get; set; }
        public long Status { get; set; }
        public string DeliveryCost { get; set; }
    }
}
