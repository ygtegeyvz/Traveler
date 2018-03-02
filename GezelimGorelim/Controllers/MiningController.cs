﻿using AForge;
using AForge.Math.Geometry;
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

        public void PointsDefinition()
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
           PointsDefinition();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double indirgenmeOrani;
            List<float> afterPointX = new List<float>();
            List<float> afterPointY = new List<float>();

            //double averageValue;
            List<double> sonuc = new List<double>();
            //float a = float.Parse(PointsY.First()) - float.Parse(PointsY.Last());
            //float b = float.Parse(PointsX.Last()) - float.Parse(PointsX.First());
            //float c = PointXFloat[0] * PointYFloat.Last() - PointXFloat.Last() * PointYFloat[0];
            //double d = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            //for (int i = 0; i < PointYFloat.Count; i++)
            //{
            //    float e = Math.Abs(a * PointXFloat[i] + b * PointYFloat[i] + c);
            //    sonuc.Add(e / d);
            //}

            //double maxValue = sonuc.Max();
            //double minValue = sonuc.Min();
            //averageValue = (maxValue + minValue) / 2;

            //for (int i = 0; i < sonuc.Count; i++)
            //{
            //    if (sonuc[i] < averageValue)
            //    {

            //        afterPointX.Add(PointXFloat[i]);
            //        afterPointY.Add(PointYFloat[i]);
            //    }
            //}
            //latLong.locationsX = afterPointX;
            //latLong.locationsY = afterPointY;
            //reductionObject.coordinate = latLong;
            //watch.Stop();
            //double timer = watch.Elapsed.TotalMilliseconds;
            //reductionObject.timer = timer;
            //indirgenmeOrani = (1 - ((double)afterPointX.Count / PointsX.Count)) * 100;
            //reductionObject.indirgenmeOrani = indirgenmeOrani;
            float maxValue;
            float minValue;
            float averageValue;
            List<float> distance = new List<float>();
            List<Point> pointsList = new List<Point>();
            Line line = Line.FromPoints(new AForge.Point(PointXFloat[0], PointYFloat[0]), new AForge.Point(PointXFloat[PointXFloat.Count - 1], PointYFloat[PointYFloat.Count - 1]));
            for (int i = 0; i < PointXFloat.Count; i++)
            {
                Point point = new Point(PointXFloat[i], PointYFloat[i]);
                pointsList.Add(point);
            }

            for (int i = 1; i < pointsList.Count-1; i++)
            {
               // string uzaklik = line.DistanceToPoint(pointsList[i]).ToString();
               //uzaklik= uzaklik.Substring(0, 7);
               // float denemeUzaklik = float.Parse(uzaklik);
                distance.Add(line.DistanceToPoint(pointsList[i]));
        }
            maxValue = distance.Max();
            minValue = distance.Min();
            averageValue = (maxValue + minValue) / 2;

            for (int i = 0; i < distance.Count; i++)
            {
                if (distance[i] <averageValue)
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
