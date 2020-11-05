using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Wasel.models
{
    public class TripRegVM
    {
        public string vehicleSequenceNumber { get; set; }
        public string driverIdentityNumber { get; set; }
        public string tripNumber { get; set; }
        public int departureLatitude { get; set; }
        public int departureLongitude { get; set; }
        public int expectedDestinationLatitude { get; set; }
        public int expectedDestinationLongitude { get; set; }
        public string departedWhen { get; set; }

    }
}
