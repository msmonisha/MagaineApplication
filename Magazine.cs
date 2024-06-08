using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagazineStore
{
    public class Magazine : IMagazine
    {
        private readonly IHttpClientFactory _clientFactory;
        private IWebProxy _proxy ;
        string tokenURL = "https://magazinestore.azurewebsites.net/api/token";
        string categoryURL = "https://magazinestore.azurewebsites.net/api/categories/{token}";
        string MagazineUrl = "https://magazinestore.azurewebsites.net/api/magazines/{token}/{category}";
        string subscriberUrl = "https://magazinestore.azurewebsites.net/api/subscribers/{token}";
        string answerUrl = "https://magazinestore.azurewebsites.net/api/answer/{token}";

        public Magazine(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        //Main method to get and return the results
        public async Task<string> GetSubscriberDatas()
        {
            List<string> subscriberIds = new List<string>();
            TokenData tokenData = await GettokendataAsync();
            Category categories = await GetCategories(tokenData.token);
            Magazines magazines = null;
            Subscribers subscribers = await GetSubscribers(categories.token);
            subscriberIds = subscribers.data.Select(item => item.id).ToList();
            for (int i = 0; i < categories.data.Count; i++)
            {
                magazines = await GetMagazines(categories.token, categories.data[i]);
                if (magazines != null)
                {
                    int[] magazineIds = magazines.data.Select(magazine => magazine.id).ToArray();
                    List<string> subscrierIDs = subscribers.data.Where(subscriber => subscriber.magazineIds.Any(id => magazineIds.Contains(id)))
                                                .Select(subscriber => subscriber.id).ToList();
                    subscriberIds = subscriberIds.Where(id => subscrierIDs.Contains(id)).ToList();
                }

            }
            string answer = await GetMagazinecodeResult(subscriberIds, magazines.token);
            return answer;
        }
        // Method to get the Token
        public async Task<TokenData> GettokendataAsync()
        {
            TokenData tokenData = null;
            _proxy = WebRequest.GetSystemWebProxy();
            _proxy.Credentials = CredentialCache.DefaultCredentials;
            var responseService = await _clientFactory.CreateClient("ProxiedClient").GetAsync(tokenURL);

            if (responseService.IsSuccessStatusCode)
            {
                var outputService = await responseService.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                try
                {
                    tokenData = JsonSerializer.Deserialize<TokenData>(outputService, options);
                }
                catch
                {
                    throw;
                }
            }

            return tokenData;
        }
        //Method to get the available categories
        public async Task<Category> GetCategories(string token)
        {
            Category category = null;
            _proxy = WebRequest.GetSystemWebProxy();
            _proxy.Credentials = CredentialCache.DefaultCredentials;
            string url = categoryURL.Replace("{token}", token);
            // Call the method Get
            var responseService = await _clientFactory.CreateClient("ProxiedClient").GetAsync(url);

            // Check the response
            if (responseService.IsSuccessStatusCode)
            {
                var outputService = await responseService.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                try
                {
                    // Deserialization of the output
                    category = JsonSerializer.Deserialize<Category>(outputService, options);
                }
                catch
                {
                    throw;
                }
            }

            return category;
        }
        //Method to get the Available magazines under categories
        public async Task<Magazines> GetMagazines(string token, string categories)
        {
            Magazines magazine = null;
            _proxy = WebRequest.GetSystemWebProxy();
            _proxy.Credentials = CredentialCache.DefaultCredentials;
            string url = MagazineUrl.Replace("{token}", token);
            url = url.Replace("{category}", categories);
            // Call the method Get
            var responseService = await _clientFactory.CreateClient("ProxiedClient").GetAsync(url);

            // Check the response
            if (responseService.IsSuccessStatusCode)
            {
                var outputService = await responseService.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                try
                {
                    // Deserialization of the output
                    magazine = JsonSerializer.Deserialize<Magazines>(outputService, options);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    throw;
                }
            }
            return magazine;
        }
        //Method to get all the subscribers
        public async Task<Subscribers> GetSubscribers(string token)
        {
            Subscribers subscribers = null;
            _proxy = WebRequest.GetSystemWebProxy();
            _proxy.Credentials = CredentialCache.DefaultCredentials;
            string url = subscriberUrl.Replace("{token}", token);
            // Call the method Get
            var responseService = await _clientFactory.CreateClient("ProxiedClient").GetAsync(url);

            // Check the response
            if (responseService.IsSuccessStatusCode)
            {
                var outputService = await responseService.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                try
                {
                    // Deserialization of the output
                    subscribers = JsonSerializer.Deserialize<Subscribers>(outputService, options);
                }
                catch
                {
                    throw;
                }
            }

            return subscribers;
        }
        //Post the subscriberid
        public async Task<string> GetMagazinecodeResult(List<string> subscriberIds, string token)
        {
            string answer = null;
            _proxy = WebRequest.GetSystemWebProxy();
            _proxy.Credentials = CredentialCache.DefaultCredentials;
            string url = answerUrl.Replace("{token}", token);
            var client = _clientFactory.CreateClient("ProxiedClient");

            // Define the request body as an object
            var requestBody = new
            {
                subscribers = subscriberIds.ToArray()
            };

            // Serialize the request body to JSON
            var json = JsonSerializer.Serialize(requestBody);

            // Create the HTTP request message with the JSON body
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the request and get the response
            var response = await client.SendAsync(request);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Handle successful response
                answer = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Result: \n" + answer);
            }
            else
            {
                // Handle unsuccessful response
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
            }

            return answer;
        }
    }
}
