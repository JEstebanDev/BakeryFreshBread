using BakeryConsoleApp.UtilsApi;
using BakeryFreshBread.Model.ListDTO;
using System.Text.Json;

namespace BakeryConsoleApp.Logic
{
    internal class Capacity
    {
        static MethodApi<BakeryOfficeList> _method;
        public Capacity()
        {
            _method = new MethodApi<BakeryOfficeList>();
        }

        public async Task<CapacityList> CheckCapacity(int id)
        {
            var result = await _method.GetById("capacity/BakeryId", id);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<CapacityList>(result, options);
        }
    }
}
