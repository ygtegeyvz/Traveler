using System.Collections.Generic;
using System.Web.Http;
using GezelimGorelim.Models;


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
