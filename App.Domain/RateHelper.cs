using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class RateHelper
    {

        public static async Task<Convertion> GetExchangeRate(string from, string to, string invoiceDate)
        {
            Convertion convertion = null;
            try
            {
                //2022-11-06
                var client = new RestClient("https://api.exchangerate.host");
                var request = new RestRequest($"/{invoiceDate}?/source=ecb&base={from}", Method.Get);
                RestResponse queryResult = await client.ExecuteAsync(request);
                convertion = JsonConvert.DeserializeObject<Convertion>(queryResult.Content);
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine(httpRequestException.StackTrace);
            }

            return convertion;
            //Examples:
            //from = "EUR"
            //to = "USD"
            //using (var client = new HttpClient())
            //{

            //    try
            //    {
            //        client.BaseAddress = new Uri("https://api.exchangerate.host");
            //        var response = await client.GetAsync($"/{invoiceDate}?source=ecb&from={from}");
            //        var stringResult = await response.Content.ReadAsStringAsync();

            //        var dictResult = JsonConvert.DeserializeObject(stringResult);
            //        return "ok";
            //        dynamic data = JObject.Parse(stringResult);
            //        //data = {"EUR_USD":{"val":1.140661}}
            //        //I want to return 1.140661
            //        //EUR_USD is dynamic depending on what from/to is
            //        return data.val;
            //    }
            //    catch (HttpRequestException httpRequestException)
            //    {
            //        Console.WriteLine(httpRequestException.StackTrace);
            //        return "Error calling API. Please do manual lookup.";
            //    }
            //}
        }

    }
}
