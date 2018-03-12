using AForge;
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

namespace GezelimGorelim.Controllers
{
    public class MiningController : ApiController
    {
        LocationController LocCont = new LocationController();
        List<Location> location = new List<Location>();
        List<double> PointsX = new List<double>();
        List<double> PointsY = new List<double>();
        List<float> PointXFloat = new List<float>();
        List<float> PointYFloat = new List<float>();
        ReductionObject reductionObject = new ReductionObject();
        LatLong latLong = new LatLong();
        //Reduction işleminden sonra kalan noktalar bu listelere atılacak.
        List<double> afterPointX = new List<double>();
        List<double> afterPointY = new List<double>();
        //Reduction işleminde kullanılmak üzere doğrusal çizgiye en yakın,en uzak ve ortalama değer tanımı.
        double maxValue;
        double minValue;
        double averageValue;
    
        //LocationController Classından alınan noktalar bir double listesine ekleniyor. 
        public void PointsDefinition()
        {
            for (int i = 0; i < LocCont.Get().Count; i++)
            {
                PointsX.Add(LocCont.Get().ElementAt(i).locationsX);
                PointsY.Add(LocCont.Get().ElementAt(i).locationsY);

            }
        }

        //api/Reduction adresinde Reduction fonksiyonu çalıştırılır.
        //Reduction fonksiyonu locationdan gelen ham verileri indirger.
        [Route("api/Reduction")]
        [HttpGet]
        public ReductionObject Reduction()
        {
            //Reduction işleminden sonra kalan noktalar bu listelere atılacak.
            //List<double> afterPointX = new List<double>();
            //List<double> afterPointY = new List<double>();
            //Reduction işleminde kullanılmak üzere doğrusal çizgiye en yakın,en uzak ve ortalama değer tanımı.
            double maxValue;
            double minValue;
            double averageValue;

            
            //Reduction Query işlemindede kullanıldığı için listelerin içi boşaltılıyor.
            if (PointsX.Count!=0)
            {
                PointsX.Clear();
                PointsY.Clear();
                PointXFloat.Clear();
                PointYFloat.Clear();
                afterPointX.Clear();
                afterPointY.Clear();
                //distance.Clear();
                //pointsList.Clear();
                
            }

            PointsDefinition();
            //Zamanlayıcı tanımlandı ve başlatıldı.
            Stopwatch watch = new Stopwatch();
            watch.Start();
            double indirgenmeOrani;
      
            //Floata çevriliyor noktalar.
            for (int i = 0; i < PointsX.Count; i++)
            {
                PointXFloat.Add((float)PointsX[i]);
                PointYFloat.Add((float)PointsY[i]);
            }
            //5 Başlangıç ve bitiş noktası belirlenip reduction işlemi yapıldı.
            AForge.Point Point1 = new AForge.Point(PointXFloat[0], PointYFloat[0]);
            AForge.Point Point2 = new AForge.Point(PointXFloat[PointYFloat.Count / 5-1], PointYFloat[PointYFloat.Count / 5-1]);

            AForge.Point Point3 = new AForge.Point(PointXFloat[(PointYFloat.Count / 5)], PointYFloat[(PointYFloat.Count / 5)]);
            AForge.Point Point4 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 2)-1], PointYFloat[(PointYFloat.Count / 5) * 2-1]);

            AForge.Point Point5 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 2)], PointYFloat[((PointYFloat.Count / 5) * 2)]);
            AForge.Point Point6 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 3)-1], PointYFloat[((PointYFloat.Count / 5) * 3)-1]);

            AForge.Point Point7 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 3)], PointYFloat[((PointYFloat.Count / 5) * 3)]);
            AForge.Point Point8 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 4)-1], PointYFloat[((PointYFloat.Count / 5) * 4)-1]);

            AForge.Point Point9 = new AForge.Point(PointXFloat[((PointYFloat.Count / 5) * 4)], PointYFloat[((PointYFloat.Count / 5) * 4)]);
            AForge.Point Point10 = new AForge.Point(PointXFloat[((PointYFloat.Count-1))], PointYFloat[(PointYFloat.Count-1)]);

            Reduction_Between_Points(Point1, Point2, 0, PointYFloat.Count / 5-1);
            Reduction_Between_Points(Point3, Point4, (PointYFloat.Count / 5), (PointYFloat.Count / 5) * 2-1);
            Reduction_Between_Points(Point5, Point6, ((PointYFloat.Count / 5) * 2)  , (PointYFloat.Count / 5) * 3-1);
            Reduction_Between_Points(Point7, Point8, ((PointYFloat.Count / 5) * 3), (PointYFloat.Count / 5) * 4-1);
            Reduction_Between_Points(Point9, Point10, ((PointYFloat.Count / 5) * 4), PointXFloat.Count - 1);


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

        //Girilen noktalar arası reduction işlemi
        public void Reduction_Between_Points(AForge.Point yeni,AForge.Point yeni2,int donguBaslangic,int donguBitis)
        {    //Noktaların doğruya uzaklığı bu listede tutulacak.
            List<double> distance = new List<double>();

            //Ayrı ayrı tutulan x ve y değerindeki veriler,point tipine dönüştürülüp bu listeye atılacak. 
            List<Point> pointsList = new List<Point>();
            //yeni = new AForge.Point(PointXFloat[PointXFloat.Count / 2 + 1], PointYFloat[PointYFloat.Count / 2 + 1]);
            Line line = Line.FromPoints(yeni,yeni2);
            ////Float tipindeki x ve y Point tipine dönüştürülüyor.
            for (int i = donguBaslangic; i < donguBitis; i++)
            {
                Point point = new Point(PointXFloat[i], PointYFloat[i]);
                pointsList.Add(point);
            }
            //// Noktaların doğruya olan uzaklığı distance listesine atılıyor.
            for (int i = 1; i < pointsList.Count - 1; i++)
            {
                distance.Add(line.DistanceToPoint(pointsList[i]));
            }
            ////Max ve minimum uzaklık değerleri ile ortalama alınıyor. 
            maxValue = distance.Max();
            minValue = distance.Min();
            averageValue = (maxValue + minValue) / 2;
            ////Eğer uzaklık ortalama uzaklıktan küçükse Reduction işleminden geçen veri olarak son listeye atılıyor.
            for (int i = 0; i < distance.Count; i++)
            {
                if (distance[i] < averageValue)
                {
                    afterPointX.Add(PointsX[donguBaslangic+i]);
                    afterPointY.Add(PointsY[donguBaslangic+i]);
                }
            }
         
        }
    }
}
