using GezelimGorelim.Controllers;
using GezelimGorelim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class HomeController : ApiController
    {
        //BURADA MADEN ÇALIŞMASI VARDIR.
        LocationController LocCont = new LocationController();
        [Route("api/Mining")]
        [HttpGet]
        public List<Location> nbr()
        {
            LocCont.Get().RemoveAt(1);
            return LocCont.Get();
        }
    }
}
