using GezelimGorelim;
using GezelimGorelim.Controllers;
using GezelimGorelim.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        List<Location> location = new List<Location>();
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

        ReductionObject reductionObject = new ReductionObject();
        LatLong latLong = new LatLong();

        [Route("api/Reduction")]
        [HttpGet]
        public ReductionObject Reduction()
        {
            bir();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double indirgenmeOrani;
            List<float> afterPointX = new List<float>();
            List<float> afterPointY = new List<float>();
            double averageValue;
            List<double> sonuc = new List<double>();
            float a = float.Parse(PointsY.First()) - float.Parse(PointsY.Last());
            float b = float.Parse(PointsX.Last()) - float.Parse(PointsX.First());
            float c = PointXFloat[0] * PointYFloat.Last() - PointXFloat.Last() * PointYFloat[0];
            double d = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            for (int i = 0; i < PointYFloat.Count; i++)
            {
                float e = Math.Abs(a * PointXFloat[i] + b * PointYFloat[i] + c);
                sonuc.Add(e / d);
            }
            //Float verinin tamamını almıyo noktadan sonra muhabbeti
            double maxValue = sonuc.Max();
            double minValue = sonuc.Min();
            averageValue = (maxValue + minValue) / 2;

            for (int i = 0; i < sonuc.Count; i++)
            {
                if (sonuc[i] < averageValue)
                {

                    afterPointX.Add(PointXFloat[i]);
                    afterPointY.Add(PointYFloat[i]);
                }
            }
            latLong.locationsX = afterPointX;
            latLong.locationsY = afterPointY;
            reductionObject.coordinate = latLong;
            watch.Stop();
            double timer = watch.Elapsed.TotalMilliseconds;
            reductionObject.timer = timer;
            indirgenmeOrani = (1 - ((double)afterPointX.Count / PointsX.Count)) * 100;
            reductionObject.indirgenmeOrani = indirgenmeOrani;

            return reductionObject;
        }
    }
}
