using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Coupons
{
    public class Coupon:FullAuditedEntity<long>
    {
        public string Code { get; set; }
        public CouponTypes CouponType { get; set; }
        public bool Active { get; set; }
    }

    public enum CouponTypes
    {
        Percentage=1,
        Value=2
    }
}
