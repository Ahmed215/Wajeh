using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.OfferPrices.Dto
{
    public class RequestWOfferPriceDto
    {
        public long OfferPriceId { get; set; }
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
        public int Status { get; set; }


        public double DeliveryCost { get; set; }
        public string AwayFrom { get; set; }
        public int DeliveryThroughDays { get; set; }
        public int DeliveryThroughHours { get; set; }
        public int DeliveryThroughMinutes { get; set; }
        public int DeliveryThroughSeconds { get; set; }
        public long RequestId { get; set; }
        public long DriverId { get; set; }
        public string DriverName { get; set; }
        public int OfferStatus { get; set; }
        public bool? IsRated { get; set; }
        public int? Rate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDriverRead { get; set; }
        public bool? IsClientRated { get; set; }
        public int? ClientRate { get; set; }

        public string ClientName { get; set; }

    }
}
