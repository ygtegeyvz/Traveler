using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GezelimGorelim.Models
{
    public class QueryModel
    {
        public string id { get; set; }
        public List<double> queryLat { get; set; }
        public List<double> queryLong { get; set; }
    }
}