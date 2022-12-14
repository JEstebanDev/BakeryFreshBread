using System.Text.Json;
using BakeryFreshBread.Model.Enum;

namespace BakeryConsoleApp.UtilsApi.Converter
{
    internal class Converter<T> : IConverter<T>
    {
        public static readonly MethodApi<T> Method = new();

        public async Task<List<T>> GetAll(string URL)
        {
            var result = await Method.GetAll(URL);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(result, options);
        }

        public async Task<T> GetById(string URL, int id)
        {
            var result = await Method.GetById(URL, id);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(result, options);
        }

        public async Task<T> Save(string URL, T data)
        {
            var result = await Method.Save(URL, data);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(result, options);
        }

        public async Task<T> UpdateOrderStatus(int id)
        {
            var URL = "office-status?id=" + id + "&status=1";
            var result = await Method.Update(URL);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(result, options);
        }
    }
}
