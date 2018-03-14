using System.Collections.Generic;
using System.Web.Http;
using GezelimGorelim.Models;


namespace GezelimGorelim.Controllers
{
    public class LocationController : ApiController
    {
        public static List<Location> reports = new List<Location> {};

        //Clienttan gönderilen veriler./api/location adresine girilmesiyle birlikte reporta ekleniyor.

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
