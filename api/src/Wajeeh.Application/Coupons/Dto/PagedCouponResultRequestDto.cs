﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Coupons.Dto
{
    public class PagedCouponResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public CouponTypes? CouponType { get; set; }
        public float? Value { get; set; }


    }
}
