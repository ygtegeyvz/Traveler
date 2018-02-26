using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GezelimGorelim.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GezelimGorelim.Controllers
{
    public class LocationController : ApiController
    {
        public static List<Location> reports = new List<Location> {};
   
        [HttpGet]
        public List<Location> Get()
        {
            return reports;
        }
        [HttpGet]
        public Location Get(string id)
        {
            return reports.Find((r) => r.id == id);
        }

        [HttpPost]
        public bool Post(Location report)
        {
            try
            {
                reports.Add(report);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
