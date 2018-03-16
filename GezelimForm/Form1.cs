using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using System.Globalization;
namespace GezelimForm
{
    public partial class Form1 : Form
    {
        List<double> latList = new List<double>();
        List<double> longList = new List<double>();

        List<string> latitudeList = new List<string>();
        List<string> longitudeList = new List<string>();

        List<double> ReductionlatitudeList = new List<double>();
        List<double> ReductionlongitudeList = new List<double>();

        List<double> QuerylatList = new List<double>();
        List<double> QuerylongList = new List<double>();

        public Form1()
        {
            InitializeComponent();
            map.MouseClick += new MouseEventHandler(map_MouseClick);
            postLocationData();
            
        }
        //Serverdaki veriyi clienta çekmemize yarayan fonksiyon
        async Task GetLocationData()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Json tipinde bir veri üstünde çalışıyoruz.
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
                    label3.Text = "Serverdan veri çekildi..";
                }

                else
                    label3.Text = "Serverdan veri çekerken hata oluştu.";
            }

        }
        //İndirgenmiş veriyi çekmemize yarayan fonksiyon.
        async Task GetReductionData()
        {
            using (var client = new HttpClient())
            {
                var result = new List<ReductionClient>();
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Json tipinde bir veri üstünde çalışıyoruz.
                HttpResponseMessage response;
                response = await client.GetAsync("api/reduction");
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
                            label2.Text = "İndirgenen veri alındı.";
                            label4.Text = "İndirgenme Süresi" + reports.timer.ToString();
                            label5.Text = "İndirgenme Oranı:" + reports.indirgenmeOrani.ToString();
                        }
                    }
                }
                else
                    label2.Text = "İndirgenmiş veri çekilirken hata oluştu.";
            }

        }
        //Sorgu sonucu oluşan veriyi çekmemize yarayan fonksiyon.
        async Task GetQueryData()
        {
            QuerylatList.Clear();
            QuerylongList.Clear();
            using (var client = new HttpClient())
            {
                var result = new List<QueryClient>();
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                response = await client.GetAsync("api/Query");
                    if (response.IsSuccessStatusCode)
                    {
                        QueryClient reports = await response.Content.ReadAsAsync<QueryClient>();
                        for (int i = 0; i < reports.queryLat.Count; i++)
                        {
                            QuerylatList.Add(reports.queryLat[i]);
                            QuerylongList.Add(reports.queryLong[i]);
                        }
                    }
                //Query Sonucu oluşan noktaları renklendirir.
                colorPoints(QuerylatList,QuerylongList,GMarkerGoogleType.green_small);             
            }

        }

        async Task GetLocationQueryData()
        {
            QuerylatList.Clear();
            QuerylongList.Clear();
            using (var client = new HttpClient())
            {
                var result = new List<QueryClient>();
                client.BaseAddress = new Uri("http://localhost:6354/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                response = await client.GetAsync("api/BasicQuery");
                if (response.IsSuccessStatusCode)
                {
                    QueryClient reports = await response.Content.ReadAsAsync<QueryClient>();
                    for (int i = 0; i < reports.queryLat.Count; i++)
                    {
                        QuerylatList.Add(reports.queryLat[i]);
                        QuerylongList.Add(reports.queryLong[i]);
                    }
                }
                //Query Sonucu oluşan noktaları renklendirir.
                colorPoints(QuerylatList, QuerylongList, GMarkerGoogleType.green_small);
            }

        }


        //Noktaları renklendirir.
        public void colorPoints(List<double>latitudeList,List<double> longitudeList,GMarkerGoogleType colour)
        {         
            List<GMapMarker> queryMarkerList = new List<GMapMarker>();
            GMapOverlay queryMarkers = new GMapOverlay("queryMarkers");
            //Data kadar döngü dönüp MarkerList oluşturuyor.Haritaya ekleniyor.
            for (int i = 0; i < latitudeList.Count; i++)
            {
                queryMarkerList.Add(new GMarkerGoogle((new PointLatLng(latitudeList[i], longitudeList[i])),
                colour)
                {
                    ToolTipText = "" + latitudeList[i] + "-" + longitudeList[i] + "+" + latitudeList.IndexOf(latitudeList[i]),
                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                });
            }
            for (int i = 0; i < queryMarkerList.Count; i++)
            {
                queryMarkers.Markers.Add(queryMarkerList[i]);
            }
            map.Overlays.Add(queryMarkers);
        }

        //Txt'den alınan verileri servera gönderir.
        async Task postLocationData()
        {
            List<string> locations = new List<string>();
            List<string> locationsX = new List<string>();
            List<string> locationsY = new List<string>();
            LocationClient newLocation = new LocationClient();
            //Txt'yi oku ve satırları locations listesine ekle.
            StreamReader reader = new StreamReader("dataset1.txt");
            string contents = reader.ReadToEnd();
            var lines = contents.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                locations.Add(line);
            }    
            reader.Close();
         
            //Virgüle kadar oku latitude listesine at virgülden sonrasını longitude listesine at.
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
                        {
                            label1.Text = "Servera veri eklendi.";

                        }

                        else
                        {
                            label1.Text = "Server'a veri eklenirken hata oluştu.";

                        }
                    }

                }
            }
            //Servera veri eklendikten sonra veri çekme işlemi başlasın.
            GetLocationData();

        }

        //Mouse'dan alınan noktaları Range Search algoritması için Query'e yollar.
        async Task postMouseClickLocation()
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
                        {

                            label7.Text = "Tamamdır";
                        }
                           
                        else
                            label7.Text = "Olmadı Kanki";
                    }
                }
            }
        }
        //Harita işlemleri.
        public void MapProcess()
        {
            if (latitudeList.Count != 0)
            {
                List<PointLatLng> Reductionpoints = new List<PointLatLng>();
                GMapOverlay routes = new GMapOverlay("routes");
                List<PointLatLng> points = new List<PointLatLng>();
                List<GMapMarker> markerList = new List<GMapMarker>();
                //Harita ayarları
                map.MapProvider = GMapProviders.GoogleMap;
                map.MaxZoom = 100;
                map.MinZoom = 1;
                map.DragButton = MouseButtons.Right;
                GMapOverlay markers = new GMapOverlay("markers");
                //Ham veri haritaya ekleniyor.
                for (int i = 0; i < latitudeList.Count; i++)
                {
                    markerList.Add(new GMarkerGoogle(new PointLatLng(double.Parse(latitudeList[i], CultureInfo.InvariantCulture.NumberFormat), double.Parse(longitudeList[i], CultureInfo.InvariantCulture.NumberFormat)),
                    GMarkerGoogleType.red_small)
                    {
                        ToolTipText = "" + latitudeList[i] + "-" + longitudeList[i] + "+" + latitudeList.IndexOf(latitudeList[i]),
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

                //Ham verinin rotası çiziliyor.
                GMapRoute route = new GMapRoute(points, "Veriler");
                route.Stroke = new Pen(Color.Red, 5);
                routes.Routes.Add(route);
                map.Overlays.Add(routes);
                ///////////////////////////////////////////////////////////////
                //İndirgenmiş veri haritaya ekleniyor.
                GMapOverlay Reductionroutes = new GMapOverlay("Reductionroutes");
                colorPoints(ReductionlatitudeList,ReductionlongitudeList,GMarkerGoogleType.blue_small);

                for (int i = 0; i < ReductionlatitudeList.Count; i++)
                {
                    Reductionpoints.Add(new PointLatLng(ReductionlatitudeList[i], ReductionlongitudeList[i]));
                }

                //İndirgenmiş verinin rotası çiziliyor.
                GMapRoute Reductionroute = new GMapRoute(Reductionpoints, "Veriler");
                Reductionroute.Stroke = new Pen(Color.Blue, 5);
                Reductionroutes.Routes.Add(Reductionroute);
                map.Overlays.Add(Reductionroutes);
            }
            else
                GetLocationData();
        }

        public void dikdortgenOlustur()
        {
            latList.Add(latList[0]);
            latList.Add(latList[1]);
            longList.Add(longList[1]);
            longList.Add(longList[0]);
            
        }

        public void dikdortgenCizdir()
        {
            GMapOverlay routes = new GMapOverlay("routes");
            List<PointLatLng> points = new List<PointLatLng>();

            points.Add(new PointLatLng(latList[0], longList[0]));
            points.Add(new PointLatLng(latList[1], longList[0]));
            points.Add(new PointLatLng(latList[1], longList[1]));
            points.Add(new PointLatLng(latList[0], longList[1]));
            points.Add(new PointLatLng(latList[0], longList[0]));

            GMapRoute route = new GMapRoute(points, "Dikdörtgen");
            route.Stroke = new Pen(Color.Black, 5);
            routes.Routes.Add(route);
            map.Overlays.Add(routes);

        }

        //Mouse tıklandığında koordinat almasını sağlayan fonksiyon.
        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            double lat;
            double lng;
            if (latList.Count < 2)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    lat = map.FromLocalToLatLng(e.X, e.Y).Lat;
                    latList.Add(lat);
                    lng = map.FromLocalToLatLng(e.X, e.Y).Lng;
                    longList.Add(lng);
                    label6.Text = ("Nokta alındı.");

                }

            }
            else if (latList.Count == 2)
            {
                dikdortgenOlustur();
                dikdortgenCizdir();
                postMouseClickLocation();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MapProcess();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            await GetReductionData();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            GetQueryData();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetLocationQueryData();
        }
    }
}