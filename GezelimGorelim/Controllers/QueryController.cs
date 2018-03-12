using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using GezelimGorelim.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Threading;


namespace GezelimGorelim.Controllers
{
    public class QueryController : ApiController
    {

        List<double> ReductionlatitudeList = new List<double>();
        List<double> ReductionlongitudeList = new List<double>();
        public static List<Query> reports = new List<Query> { };
        QueryModel queryObject = new QueryModel();
        //public void Inserting()
        //{
        //    for (int i = 0; i < 100000000; i++)
        //    {
        //    }
        //}



        //public async Task nbrAsync()
        //{

        //    await GetReductionRequest("0");
        //    Inserting();

        //}

        //[HttpGet]
        //public List<Query> Get()
        //{
        //    return reports;
        //}

        [HttpGet]
        public QueryModel Get()
        {
                //var result = new List<ReductionObject>();
                //client.BaseAddress = new Uri("http://localhost:6354/");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response;
                //response = await client.GetAsync("api/reduction");

                //if (response.IsSuccessStatusCode)
                //{
                //    ReductionObject reports = await response.Content.ReadAsAsync<ReductionObject>();
                //    //  foreach (var report in reports)
                //    {
                //        for (int i = 0; i < reports.coordinate.locationsX.Count; i++)
                //        {
                //            ReductionlatitudeList.Add(reports.coordinate.locationsX[i]);
                //            ReductionlongitudeList.Add(reports.coordinate.locationsY[i]);

                //        }
                //    }
                //}

                //KDTree t = new KDTree(new Points(ReductionlatitudeList[0], ReductionlongitudeList[0]), 2);
                //for (int i = 0; i < ReductionlatitudeList.Count; i++)
                //{
                //    t.Insert(new GezelimGorelim.KDNode(new GezelimGorelim.Points(ReductionlatitudeList[i], ReductionlongitudeList[i])), t.Root);
                //}
                //double searchPoint1x = double.Parse(reports[0].locationsX);
                //double searchPoint1y = double.Parse(reports[0].locationsY);
                //double searchPoint2x = double.Parse(reports[1].locationsX);
                //double searchPoint2y = double.Parse(reports[1].locationsY);
                //Points find = new GezelimGorelim.Points(searchPoint1x, searchPoint1y);
                //Points find2 = new Points(searchPoint2x, searchPoint2y);
                ////Points find = new Points(39.907422, 116.3);
                ////    Points find2 = new Points(40.018998, 116.320);


                //List<double> queryLat = new List<double>();
                //List<double> queryLong = new List<double>();
                ////KDNode found = t.NNSearch(find);
                //List<KDNode> found = t.RangeSearch(find, find2);
                ////En yakın noktayı buluyor.
                //List<Points> foundData = new List<Points>();
                //for (int i = 0; i < found.Count; i++)
                //{
                //    foundData.Add(found[i].data);
                //    queryLat.Add(foundData[i].latitude);
                //    queryLong.Add(foundData[i].longitude);

                //}
                //queryObject.queryLat = queryLat;
                //queryObject.queryLong = queryLong;
                MiningController mine = new MiningController();
                {
                    for (int i = 0; i < mine.Reduction().coordinate.locationsX.Count; i++)
                    {
                        ReductionlatitudeList.Add(mine.Reduction().coordinate.locationsX[0]);
                        ReductionlongitudeList.Add(mine.Reduction().coordinate.locationsY[0]);
                    }
                }


                KDTree t = new KDTree(new Points(ReductionlatitudeList[0], ReductionlongitudeList[0]), 2);
                for (int i = 0; i < ReductionlatitudeList.Count; i++)
                {
                    t.Insert(new GezelimGorelim.KDNode(new GezelimGorelim.Points(ReductionlatitudeList[i], ReductionlongitudeList[i])), t.Root);
                }
                double searchPoint1x = double.Parse(reports[0].locationsX);
                double searchPoint1y = double.Parse(reports[0].locationsY);
                double searchPoint2x = double.Parse(reports[1].locationsX);
                double searchPoint2y = double.Parse(reports[1].locationsY);
                Points find = new GezelimGorelim.Points(searchPoint1x, searchPoint1y);
                Points find2 = new Points(searchPoint2x, searchPoint2y);
                //Points find = new Points(39.907422, 116.3);
                //    Points find2 = new Points(40.018998, 116.320);


                List<double> queryLat = new List<double>();
                List<double> queryLong = new List<double>();
                //KDNode found = t.NNSearch(find);
                List<KDNode> found = t.RangeSearch(find, find2);
                //En yakın noktayı buluyor.
                List<Points> foundData = new List<Points>();
                for (int i = 0; i < found.Count; i++)
                {
                    foundData.Add(found[i].data);
                    queryLat.Add(foundData[i].latitude);
                    queryLong.Add(foundData[i].longitude);

                }
                queryObject.queryLat = queryLat;
                queryObject.queryLong = queryLong;

                return queryObject;

            
        }


        [HttpPost]
        public bool Post(Query report)
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