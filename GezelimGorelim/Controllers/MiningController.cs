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

    //    //public List<Location> reduction()
    //    //{
    //    //    float x, y;

    //    //    List<float> points = new List<float>();
    //    //    List<float> afterPoint = new List<float>();

    //    //    float pay = (y - points[points.Count]) * (points[points.Count] - points[0]);
    //    //    float payda = (points[points.Count] - points[0]) * (x - points[points.Count]);

    //    //    if (pay / payda < 3)
    //    //    {

    //    //        afterPoint.Add(points);

    //    //    }

    //}
}
}
