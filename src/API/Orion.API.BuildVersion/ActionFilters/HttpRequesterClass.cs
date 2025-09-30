using System.Net.Http.Headers;

namespace Orion.API.BuildVersion.ActionFilters
{
    public static class HttpRequesterClass
    {
        /// <summary>
        /// Store the client key to be used through the program
        /// </summary>
        static string _clientKey = "guest:guest";

        /// <summary>
        /// Method to make HTTP calls to TradingEconomics API
        /// </summary>
        /// <param name="url">The URL to fetch</param>
        /// <param name="baseUrl">The base path, the default is 'https://api.tradingeconomics.com/'</param>
        /// <returns>A task tha will be resolved in a string with the content of the response</returns>
        public static async Task<string> HttpRequester(string url, string baseUrl = "https://api.tradingeconomics.com/")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Clear();

                    //ADD Acept Header to tell the server what data type you want
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    //ADD Authorization
                    AuthenticationHeaderValue auth = new AuthenticationHeaderValue("Client", _clientKey);
                    client.DefaultRequestHeaders.Authorization = auth;

                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                return $"Error at HttpRequester with msg: {e}";
            }
        }
    }
}
