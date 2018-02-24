using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GezelimGorelim.Models;

namespace GezelimGorelim.Controllers
{
    public class LocationController : ApiController
    {
        public static List<Location> reports = new List<Location>
        {
            //new Location { id="1", latitude = 8654465456, longitude = 1134594655},
            //new Location { id="2", latitude = 2155465456, longitude = 4654594654},
          
        };

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
