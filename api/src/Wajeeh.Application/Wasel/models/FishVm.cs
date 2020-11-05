using System;
using System.Collections.Generic;
using System.Text;

namespace Wajeeh.Wasel.models
{
    public class FishVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string nameAR { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int FishId { get; set; }
        public double AvgPrice { get; set; }
        public int fishFamily { get; set; }
        public string descriptionAR { get; set; }
        public string token { get; set; }
    }
}
