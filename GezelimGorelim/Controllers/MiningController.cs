using GezelimGorelim.Controllers;
using GezelimGorelim.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            bir();
            reduction();
            LocCont.Get().RemoveAt(1);

            return LocCont.Get();
        }

        List<Location> location = new List<Location>();

        //[Route("api/Reduction")]
        //[HttpGet]
        //public List<Location> reduction()
        //{
        //    location = LocCont.Get();

        //    return location;

        //}

        List<string> PointsX = new List<string>();
        List<string> PointsY = new List<string>();
        List<float> PointXFloat = new List<float>();
        List<float> PointYFloat = new List<float>();

        public void bir()
        {
            for (int i = 0; i < LocCont.Get().Count; i++)
            {
                PointsX.Add(LocCont.Get().ElementAt(i).locationsX);
                PointsY.Add(LocCont.Get().ElementAt(i).locationsY);

            }

            for (int i = 0; i < PointsX.Count; i++)
            {
               PointXFloat.Add(float.Parse(PointsX[i],
                         CultureInfo.InvariantCulture.NumberFormat));
                PointYFloat.Add(float.Parse(PointsY[i],
                         CultureInfo.InvariantCulture.NumberFormat));

            }

        }


        public void reduction()
        {

          //  float x, y;
            List<float> afterPointX = new List<float>();
            List<float> afterPointY = new List<float>();

            double a = PointXFloat[PointXFloat.Count-1] - PointXFloat[0];
            double b = PointYFloat[PointYFloat.Count-1] - PointYFloat[0];
            double boy = Math.Sqrt((a * a) + (b * b));

            for (int i = 0; i < PointsX.Count; i++)
            {
                float pay = (PointYFloat[i] - PointYFloat[PointYFloat.Count-1]) * (PointXFloat[PointXFloat.Count-1] - PointXFloat[0]);
                float payda = (PointYFloat[PointYFloat.Count-1] - PointYFloat[0]) * (PointXFloat[i] - PointXFloat[PointXFloat.Count-1]);
                              
                
                double kontrol =   Math.Abs(pay/payda) / boy;

                if(kontrol>60 && kontrol < 80)
                {
                    afterPointX.Add(PointXFloat[i]);
                    afterPointY.Add(PointYFloat[i]);
                }

            }

        }
    }
}
