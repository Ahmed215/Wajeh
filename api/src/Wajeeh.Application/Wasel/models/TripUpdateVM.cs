using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Wasel.models
{
    public class TripUpdateVM
    {
        public int actualDestinationLatitude { get; set; }
        public int actualDestinationLongitude { get; set; }
        public string arrivedWhen { get; set; }
        public int tripNumber { get; set; }


    }
}
