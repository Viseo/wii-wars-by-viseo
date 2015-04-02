using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Viseo.WiiWars.WiimoteInSpace.WebAPI
{
    public class Client
    {

        public async void GetWiimote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Properties.Resources.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/saber");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    //Product product = await response.Content.ReadAsAsync > Product > ();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }
            }
        }
    }
}
