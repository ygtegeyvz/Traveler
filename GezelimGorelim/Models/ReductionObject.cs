using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GezelimGorelim.Models
{
    public class ReductionObject
    {
        public LatLong coordinate { get; set; }
        public double timer { get; set; }
        public double indirgenmeOrani { get; set; }
        public int Id { get; set; }
    }
}