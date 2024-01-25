using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static UserMangement.Models.ApiResponse;

namespace UserMangement.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {

        }

        public async Task<Cart> CartAsync(int id)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://dummyjson.com/carts/{id}");

            request.Headers.Add("Authorization", "Basic Og==");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            if (response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();
                if(content != null)
                {
                    var myDeserializedClass = JsonConvert.DeserializeObject<Cart>(content);

                    return myDeserializedClass;
                }
               
            }
            return null;

        }

        public async Task<AllRoot> AllCartAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/carts/");
            request.Headers.Add("Authorization", "Basic Og==");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            if (response.Content != null)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != null)
                {
                    var myDeserializedClass = JsonConvert.DeserializeObject<AllRoot>(content);

                    return myDeserializedClass;
                }

            }
            return null;

        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Product
        {
            public int id { get; set; }
            public string title { get; set; }
            public int price { get; set; }
            public int quantity { get; set; }
            public int total { get; set; }
            public double discountPercentage { get; set; }
            public int discountedPrice { get; set; }
            public string thumbnail { get; set; }
        }

        public class Root 
        {
            public int id { get; set; }
            public List<Product> products { get; set; }
            public int total { get; set; }
            public int discountedTotal { get; set; }
            public int userId { get; set; }
            public int totalProducts { get; set; }
            public int totalQuantity { get; set; }
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Cart
        {
            public int id { get; set; }
            public List<Product> products { get; set; }
            public int total { get; set; }
            public int discountedTotal { get; set; }
            public int userId { get; set; }
            public int totalProducts { get; set; }
            public int totalQuantity { get; set; }
        }


        public class AllRoot
        {
            public List<Cart> carts { get; set; }
            public int total { get; set; }
            public int skip { get; set; }
            public int limit { get; set; }
        }

    }
}

