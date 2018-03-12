using GezelimGorelim.Models;
using System.Collections.Generic;
using System.Web.Http;


namespace GezelimGorelim.Controllers
{
    public class QueryController : ApiController
    {

        List<double> ReductionlatitudeList = new List<double>();
        List<double> ReductionlongitudeList = new List<double>();
        public static List<Query> reports = new List<Query> { };
        QueryModel queryObject = new QueryModel();
     
        [HttpGet]
        public QueryModel Get()
        {
            MiningController mine = new MiningController();
            {
                for (int i = 0; i < mine.Reduction().coordinate.locationsX.Count; i++)
                {
                    ReductionlatitudeList.Add(mine.Reduction().coordinate.locationsX[i]);
                    ReductionlongitudeList.Add(mine.Reduction().coordinate.locationsY[i]);
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
            Points find = new Points(searchPoint1x, searchPoint1y);
            Points find2 = new Points(searchPoint2x, searchPoint2y);
          
            List<double> queryLat = new List<double>();
            List<double> queryLong = new List<double>();

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