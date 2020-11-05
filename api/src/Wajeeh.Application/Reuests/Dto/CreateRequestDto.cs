using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Requests;

namespace Wajeeh.Reuests.Dto
{
    [AutoMap(typeof(Request))]
    public class CreateRequestDto
    {
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
        public string DeliveryCost { get; set; }
        public int Status { get; set; }
    }
}
