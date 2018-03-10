using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using System.Globalization;
using System.Net;

namespace GezelimForm
{
    public partial class Form1 : Form
    {
        List<double> latList = new List<double>();
        List<double> longList = new List<double>();

        List<string> latitudeList = new List<string>();
        List<string> longitudeList = new List<string>();

        List<float> ReductionlatitudeList = new List<float>();
        List<float> ReductionlongitudeList = new List<float>();

        List<double> QuerylatList = new List<double>();
        List<double> QuerylongList = new List<double>();

        public Form1()
        {
            InitializeComponent();
            map.MouseClick += new MouseEventHandler(map_MouseClick);
        }


        async Task GetRequest(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                if (id == "0")
                {
                    response = await client.GetAsync("api/Location");
                    if (response.IsSuccessStatusCode)
                    {
                        LocationClient[] reports = await response.Content.ReadAsAsync<LocationClient[]>();
                        foreach (var report in reports)
                        {
                            //     Debug.Write("" + report.location);
                            latitudeList.Add(report.locationsX);
                            longitudeList.Add(report.locationsY);
                            label3.Text = "Tamamdır.";
                        }
                    }
                }
                else
                {
                    response = await client.GetAsync("api/Location/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        LocationClient report = await response.Content.ReadAsAsync<LocationClient>();
                        //   Debug.Write("" + report.location);
                        latitudeList.Add(report.locationsX);
                        longitudeList.Add(report.locationsY);
                        label3.Text = "Tamamdır.";
                    }
                }
            }

        }

        async Task GetRequest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;
                response = await client.GetAsync("api/Location");
                if (response.IsSuccessStatusCode)
                {
                    LocationClient[] reports = await response.Content.ReadAsAsync<LocationClient[]>();
                    foreach (var report in reports)
                    {
                        latitudeList.Add(report.locationsX);
                        longitudeList.Add(report.locationsY);
                    }
                }
            }

        }

        async Task GetReductionRequest(string id)
        {
            using (var client = new HttpClient())
            {
                var result = new List<ReductionClient>();
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
                         ReductionClient reports = await response.Content.ReadAsAsync<ReductionClient>();
                      //  foreach (var report in reports)
                        {
                            for (int i = 0; i < reports.coordinate.locationsX.Count; i++)
                            {
                                ReductionlatitudeList.Add(reports.coordinate.locationsX[i]);
                                ReductionlongitudeList.Add(reports.coordinate.locationsY[i]);
                                label2.Text = "Tamamdır.";
                                label4.Text = "Süre" + reports.timer.ToString();
                                label5.Text = "Oran:" + reports.indirgenmeOrani.ToString();
                            }
                        }
                    }
                }


            }

        }

        //async Task GetQueryRequest(string id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var result = new List<Query>();
        //        client.BaseAddress = new Uri("http://localhost:6354/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response;
        //        response = await client.GetAsync("api/Query");
        //        if (id == "0")
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // BURADA HATA VAR
        //                QueryClient[] reports = await response.Content.ReadAsAsync<QueryClient[]>();
        //                foreach (var report in reports)
        //                {
        //                   QuerylatList.Add(report.latitude);
        //                   QuerylongList.Add(report.longitude);
        //                }
        //            }
        //        }


        //    }

        //}

        async Task postRequest()
        {
            // string file_way = @"D:\githubRepo\Traveler\GezelimForm\bin\Debug\Dataset.txt";
             //string file_way = @"C:\Belgeler\GitHub\Traveler\Dataset.txt";
             //FileStream fs = new FileStream(file_way, FileMode.OpenOrCreate, FileAccess.Write);
            List<string> locations = new List<string>();
            List<string> locationsX = new List<string>();
            List<string> locationsY = new List<string>();
            StreamReader reader = new StreamReader("dataset1.txt");
            string contents = reader.ReadToEnd();
            var lines = contents.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                locations.Add(line);
            }

            reader.Close();
            LocationClient newLocation = new LocationClient();

            for (int i = 0; i < locations.Count; i++)
            {
                string[] point = locations[i].Split(',');

                locationsX.Add(point[0]);
                locationsY.Add(point[1]);

                Array.Clear(point, 0, point.Length);
            }

            for (int i = 0; i < locations.Count; i++)
            {
                newLocation.locationsX = locationsX[i];
                newLocation.locationsY = locationsY[i];

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:6354/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Location", newLocation);

                    if (response.IsSuccessStatusCode)
                    {
                        bool result = await response.Content.ReadAsAsync<bool>();
                        if (result)
                            label1.Text = "Tamamdır.";
                        else
                            label1.Text = "Olmadı Kanki";
                    }
                }
            }
        }


        async Task postLocation()
        {
            LocationClient newLocation = new LocationClient();
            for (int i = 0; i < latList.Count; i++)
            {
                newLocation.locationsX = latList[i].ToString();
                newLocation.locationsY = longList[i].ToString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:6354/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
  
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/Query", newLocation);
                    if (response.IsSuccessStatusCode)
                    {
                        bool result = await response.Content.ReadAsAsync<bool>();
                        if (result)
                            label7.Text = "Tamamdır.";
                        else
                            label7.Text = "Olmadı Kanki";
                    }
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            GetRequest(textBox1.Text);
        }

        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            await postRequest();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // GetRequest("0");
            // Çakışma oluyo Serverdan yanıt gelmıyo

            GMapOverlay routes = new GMapOverlay("routes");
            List<PointLatLng> points = new List<PointLatLng>();
            List<GMapMarker> markerList = new List<GMapMarker>();
            map.MapProvider = GMapProviders.GoogleMap;
            map.MaxZoom = 100;
            map.MinZoom = 1;
            map.DragButton = MouseButtons.Right;
            GMapOverlay markers = new GMapOverlay("markers");
            for (int i = 0; i < latitudeList.Count; i++)
            {
                markerList.Add(new GMarkerGoogle(new PointLatLng(double.Parse(latitudeList[i], CultureInfo.InvariantCulture.NumberFormat), double.Parse(longitudeList[i], CultureInfo.InvariantCulture.NumberFormat)),
                GMarkerGoogleType.red_small)
                {
                    ToolTipText = "" + latitudeList[i] + "-" + longitudeList[i]+"+"+latitudeList.IndexOf(latitudeList[i]),
                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                });
            }
            for (int i = 0; i < markerList.Count; i++)
            {
                markers.Markers.Add(markerList[i]);
            }
            map.Overlays.Add(markers);

            for (int i = 0; i < latitudeList.Count; i++)
            {
                points.Add(new PointLatLng(double.Parse(latitudeList[i], CultureInfo.InvariantCulture.NumberFormat), double.Parse(longitudeList[i], CultureInfo.InvariantCulture.NumberFormat)));
            }


            GMapRoute route = new GMapRoute(points,"Veriler");
            route.Stroke = new Pen(Color.Red, 5);
            routes.Routes.Add(route);
            map.Overlays.Add(routes);
            ///////////////////////////////////////////////////////////////

            GMapOverlay Reductionroutes = new GMapOverlay("Reductionroutes");
            List<PointLatLng> Reductionpoints = new List<PointLatLng>();
            List<GMapMarker> ReductionmarkerList = new List<GMapMarker>();
            GMapOverlay Reductionmarkers = new GMapOverlay("Reductionmarkers");
            for (int i = 0; i < ReductionlatitudeList.Count; i++)
            {
                ReductionmarkerList.Add(new GMarkerGoogle((new PointLatLng(ReductionlatitudeList[i],ReductionlongitudeList[i])),
                GMarkerGoogleType.blue_small)
                {
                    ToolTipText = "" + ReductionlatitudeList[i] + "-" + ReductionlongitudeList[i]+"+"+ReductionlatitudeList.IndexOf(ReductionlatitudeList[i]),
                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                });
            }
            for (int i = 0; i < ReductionmarkerList.Count; i++)
            {
                Reductionmarkers.Markers.Add(ReductionmarkerList[i]);
            }
            map.Overlays.Add(Reductionmarkers);

            for (int i = 0; i < ReductionlatitudeList.Count; i++)
            {
                Reductionpoints.Add(new PointLatLng(ReductionlatitudeList[i],ReductionlongitudeList[i]));
            }


            GMapRoute Reductionroute = new GMapRoute(Reductionpoints, "Veriler");
            Reductionroute.Stroke = new Pen(Color.Blue, 5);
            Reductionroutes.Routes.Add(Reductionroute);
            map.Overlays.Add(Reductionroutes);
        }

        private void gmap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            Console.WriteLine(String.Format("Marker {0} was clicked.", item.Tag));
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await GetReductionRequest("0");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            postLocation();
        }
 
        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (latList.Count < 2)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    double lat = map.FromLocalToLatLng(e.X, e.Y).Lat;
                    latList.Add(lat);
                    double lng = map.FromLocalToLatLng(e.X, e.Y).Lng;
                    longList.Add(lng);
                    label6.Text = ("Nokta alındı.");
                }
            }
           

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //GetQueryRequest("0");
        }
    }
}