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
namespace GezelimForm
{
    public partial class Form1 : Form
    {
     
        public Form1()
        {
            InitializeComponent();
            
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
                            Debug.Write("" + report.location);
                
                        }    
                            }    
                        }
                else
                {
                    response = await client.GetAsync("api/Location/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        LocationClient report = await response.Content.ReadAsAsync<LocationClient>();
                        Debug.Write("" + report.location);
                
                    }
                } 
            }

        }

        async Task postRequest()
        {
            string file_way = @"D:\Projeler\GezelimGorelim\Dataset.txt";
            FileStream fs = new FileStream(file_way, FileMode.OpenOrCreate, FileAccess.Write);
            List<string> locations = new List<string>();
            StreamReader reader = new StreamReader("Dataset.txt");
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
                newLocation.location = locations[i];
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



        private void button2_Click(object sender, EventArgs e)
        {
            GetRequest(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            postRequest();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}