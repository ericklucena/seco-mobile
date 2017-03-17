using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SecoMobile.Services
{
    class Network
    {
        private const string GET_SECO_SENSOR = "http://seco-hc4.appspot.com/manhole/get";
        private const string MAINTENANCE_SECO_SENSOR = "http://seco-hc4.appspot.com/manhole/maintenance?id=";
        private const string SECO_INSERTION = "http://seco-hc4.appspot.com/manhole/insert?id={0}&name={1}&street={2}&latitude={3}&longitude={4}&dimensions={5}";

        public static async Task<string> SecoData()
        {
            return await GetResponse(GET_SECO_SENSOR+"?time="+DateTime.Now);
        }

        public static async Task<string> SecoInsertion(string id, string name, string street, string lat, string lng, string x, string y, string z)
        {
            string request = string.Format(SECO_INSERTION, id, name, street, lat, lng, string.Format("{0}:{1}:{2}", x, y, z));
            return await GetResponse(request);
        }

        public static async Task<string> RegisterMaintenance(int id)
        {
            string response = await GetResponse(MAINTENANCE_SECO_SENSOR + id);
            return response;
        }


        private static async Task<string> GetResponse(string url)
        {
            string webresponse = null;
            try
            {
                HttpClient http = new System.Net.Http.HttpClient();
                HttpResponseMessage response = await http.GetAsync(url);
                webresponse = await response.Content.ReadAsStringAsync();

            }
            catch (Exception e)
            {
                throw e;
            }

            return webresponse;
        }
    }
}