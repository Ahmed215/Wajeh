using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Authorization.Users;
using Wajeeh.OfferPrices;
using Wajeeh.Subcategories;

namespace Wajeeh.Requests
{
    public class Request : FullAuditedEntity<long>
    {
        public bool? IsReg { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
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
        [ForeignKey("Subcategory")]
        public long SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }
        
        public long UserRequsetId { get; set; }
        [ForeignKey("UserRequsetId")]
        public User UserRequset { get; set; }

        public int Status { get; set; }
        public string DeliveryCost { get; set; }
        public virtual ICollection<OfferPrice> OfferPrices { get; set; }
    }
}
