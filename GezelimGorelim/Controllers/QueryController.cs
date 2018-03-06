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
    public class queryController : ApiController
    {

        List<double> ReductionlatitudeList = new List<double>();
        List<double> ReductionlongitudeList = new List<double>();

        public void Inserting()
        {
            for (int i = 0; i < 100000000; i++)
            {

            }
          

        }
        async Task  GetReductionRequest(string id)
        {
            using (var client = new HttpClient())
            {
                var result = new List<ReductionObject>();
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                //HttpWebRequest request = WebRequest.Create("http://localhost:6354/api/reduction") as HttpWebRequest;
                //string jsonVerisi = "";
                //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                //{
                //    StreamReader reader = new StreamReader(response.GetResponseStream());
                //    //jsonVerisi adlı değişkene elde ettiği veriyi atıyoruz.
                //    jsonVerisi = reader.ReadToEnd();
                //}

                HttpResponseMessage response;
                response = await client.GetAsync("api/reduction");
                if (id == "0")
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // BURADA HATA VAR
                        ReductionObject reports = await response.Content.ReadAsAsync<ReductionObject>();
                        //  foreach (var report in reports)
                        {
                            for (int i = 0; i < reports.coordinate.locationsX.Count; i++)
                            {
                                ReductionlatitudeList.Add(reports.coordinate.locationsX[i]);
                                ReductionlongitudeList.Add(reports.coordinate.locationsY[i]);
                                
                            }
                        }
                    }
                }
                KDTree t = new KDTree(new Points(ReductionlatitudeList[0], ReductionlongitudeList[0]), 2);
                for (int i = 0; i < ReductionlatitudeList.Count; i++)
                {
                    t.Insert(new KDNode(new Points(ReductionlatitudeList[i], ReductionlongitudeList[i])), t.Root);
                }
                Points find = new Points(39.907422, 116.37319);

                KDNode found = t.NNSearch(find);
                 //En yakın noktayı buluyor.
              

            }

        }
        [Route("api/query")]
        [HttpGet]
       public void nbr()
        {
            
            GetReductionRequest("0");
            Inserting();

        }





    }

    
}