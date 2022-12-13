using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BakeryFreshBread.Error;

namespace BakeryConsoleApp.UtilsApi
{
    internal class MethodApi<T> : IMethodApi<T>
    {
        static readonly HttpClient client = new();
        private const string BASEURL = "https://localhost:7284/";

        public async Task<string> GetAll(string URL)
        {
            try
            {
                using var response = await client.GetAsync(BASEURL + URL);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException)
            {
                throw new CustomException(HttpStatusCode.BadRequest, "Error sending the request!");
            }
        }

        public async Task<string> GetById(string URL, int id)
        {
            try
            {
                using var response = await client.GetAsync(BASEURL + URL + "/" + id);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException)
            {
                throw new CustomException(HttpStatusCode.BadRequest, "Error sending the request!");
            }
        }

        public async Task<string> Save(string URL, T data)
        {
            try
            {
                JsonContent content = JsonContent.Create(data);
                using var response = await client.PostAsync(BASEURL + URL, content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (HttpRequestException)
            {
                throw new CustomException(HttpStatusCode.BadRequest, "Error sending the request!");
            }
        }

        public async Task<string> Update(int id, T data)
        {
            throw new NotImplementedException();
        }
    }
}
