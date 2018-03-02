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
        public static List<Reduction> reports = new List<Reduction> { };
        [Route("api/Mining")]
        [HttpGet]
        public List<Location> nbr()
        {
            //  bir();
            //Reduction();
            LocCont.Get().RemoveAt(1);

            return LocCont.Get();
        }

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

        //[Route("api/Reduction")]
        //[HttpGet]
        //public Tuple<List<float>, List<float>, double, double> Reduction()
        //{
        //    bir();
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    double indirgenmeOrani;
        //    List<float> afterPointX = new List<float>();
        //    List<float> afterPointY = new List<float>();
        //    //  List<double> kontrol = new List<double>();
        //    double averageValue;
        //    //  double a = PointXFloat[PointXFloat.Count-1] - PointXFloat[0];
        //    //  double b = PointYFloat[PointYFloat.Count-1] - PointYFloat[0];
        //    //  double boy = Math.Sqrt((a * a) + (b * b));

        //    //  for (int i = 0; i < PointsX.Count; i++)
        //    //  {
        //    //      float pay = (PointYFloat[i] - PointYFloat[PointYFloat.Count-1]) * (PointXFloat[PointXFloat.Count-1] - PointXFloat[0]);
        //    //      float payda = (PointYFloat[PointYFloat.Count-1] - PointYFloat[0]) * (PointXFloat[i] - PointXFloat[PointXFloat.Count-1]);
        //    //      //Datasetin boyutuna göre uzaklık değerleri değişiyor formül yanlış


        //    //      kontrol.Add(Math.Abs(pay / payda) / boy );


        //    //  }
        //    //  double maxValue = kontrol.Max();
        //    //  double minValue = kontrol.Min();
        //    //  averageValue = (maxValue + minValue) / 2;
        //    //  for (int i = 0; i <kontrol.Count ; i++)
        //    //  {
        //    //      if (kontrol[i] < averageValue)
        //    //      {
        //    //          afterPointX.Add(PointXFloat[i]);
        //    //          afterPointY.Add(PointYFloat[i]);
        //    //      }
        //    //  }
        //    List<double> sonuc = new List<double>();
        //    float a = float.Parse(PointsY.First()) - float.Parse(PointsY.Last());
        //    float b = float.Parse(PointsX.Last()) - float.Parse(PointsX.First());
        //    float c = PointXFloat[0] * PointYFloat.Last() - PointXFloat.Last() * PointYFloat[0];
        //    double d = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        //    for (int i = 0; i < PointYFloat.Count; i++)
        //    {
        //        float e = Math.Abs(a * PointXFloat[i] + b * PointYFloat[i] + c);
        //        sonuc.Add(e / d);
        //    }
        //    //Float verinin tamamını almıyo noktadan sonra muhabbeti
        //    double maxValue = sonuc.Max();
        //    double minValue = sonuc.Min();
        //    averageValue = (maxValue + minValue) / 2;
        //    for (int i = 0; i < sonuc.Count; i++)
        //    {
        //        if (sonuc[i] < averageValue)
        //        {
        //            afterPointX.Add(PointXFloat[i]);
        //            afterPointY.Add(PointYFloat[i]);
        //        }
        //    }
        //    watch.Stop();
        //    double timer = watch.Elapsed.TotalMilliseconds;
        //    indirgenmeOrani = (1 - ((double)afterPointX.Count / PointsX.Count)) * 100;

        //    return Tuple.Create(afterPointX, afterPointY, indirgenmeOrani, timer);
        //}

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
            //  List<double> kontrol = new List<double>();
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

        //[Route("api/Reduction")]
        //[HttpGet]
        //public List<Reduction> Reduction()
        //{
        //    bir();
        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();
        //    double indirgenmeOrani;
        //    List<float> afterPointX = new List<float>();
        //    List<float> afterPointY = new List<float>();
        //    //  List<double> kontrol = new List<double>();
        //    double averageValue;
        //    //  double a = PointXFloat[PointXFloat.Count-1] - PointXFloat[0];
        //    //  double b = PointYFloat[PointYFloat.Count-1] - PointYFloat[0];
        //    //  double boy = Math.Sqrt((a * a) + (b * b));

        //    //  for (int i = 0; i < PointsX.Count; i++)
        //    //  {
        //    //      float pay = (PointYFloat[i] - PointYFloat[PointYFloat.Count-1]) * (PointXFloat[PointXFloat.Count-1] - PointXFloat[0]);
        //    //      float payda = (PointYFloat[PointYFloat.Count-1] - PointYFloat[0]) * (PointXFloat[i] - PointXFloat[PointXFloat.Count-1]);
        //    //      //Datasetin boyutuna göre uzaklık değerleri değişiyor formül yanlış


        //    //      kontrol.Add(Math.Abs(pay / payda) / boy );


        //    //  }
        //    //  double maxValue = kontrol.Max();
        //    //  double minValue = kontrol.Min();
        //    //  averageValue = (maxValue + minValue) / 2;
        //    //  for (int i = 0; i <kontrol.Count ; i++)
        //    //  {
        //    //      if (kontrol[i] < averageValue)
        //    //      {
        //    //          afterPointX.Add(PointXFloat[i]);
        //    //          afterPointY.Add(PointYFloat[i]);
        //    //      }
        //    //  }
        //    List<double> sonuc = new List<double>();
        //    float a = float.Parse(PointsY.First()) - float.Parse(PointsY.Last());
        //    float b = float.Parse(PointsX.Last()) - float.Parse(PointsX.First());
        //    float c = PointXFloat[0] * PointYFloat.Last() - PointXFloat.Last() * PointYFloat[0];
        //    double d = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        //    for (int i = 0; i < PointYFloat.Count; i++)
        //    {
        //        float e = Math.Abs(a * PointXFloat[i] + b * PointYFloat[i] + c);
        //        sonuc.Add(e / d);
        //    }
        //    //Float verinin tamamını almıyo noktadan sonra muhabbeti
        //    double maxValue = sonuc.Max();
        //    double minValue = sonuc.Min();
        //    averageValue = (maxValue + minValue) / 2;
        //    for (int i = 0; i < sonuc.Count; i++)
        //    {
        //        if (sonuc[i] < averageValue)
        //        {
        //            afterPointX.Add(PointXFloat[i]);
        //            afterPointY.Add(PointYFloat[i]);
        //        }
        //    }
        //    watch.Stop();
        //    double timer = watch.Elapsed.TotalMilliseconds;
        //    indirgenmeOrani = (1 - ((double)afterPointX.Count / PointsX.Count)) * 100;

        //    return reports;
        //}


    }
}
