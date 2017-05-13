using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ClientSide
{
    public class ProductService
    {
        public static List<ProductDTO> GetListProduct()
        {
            List<ProductDTO> result;
            const string url = "http://localhost/ConsumingAPI/api/Products ";

            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                var data = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<ProductDTO>>(data);
            }
            return result;
        }

        public static void InsertProduct()
        {
            const string url = "http://localhost/ConsumingAPI/api/Products";

            var product = new ProductDTO
            {
                ProductID = 0,
                ProductName = "Test ProductName",
                SupplierID = 1,
                CategoryID = 1,
                QuantityPerUnit = "10",
                UnitPrice = 1000,
                UnitsInStock = 6,
                UnitsOnOrder = 10,
                ReorderLevel = 1
            };

            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(url, content).Result;
            }
        }
    }
}
