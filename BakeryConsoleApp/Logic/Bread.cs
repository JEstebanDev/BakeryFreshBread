using BakeryConsoleApp.UtilsApi;
using BakeryFreshBread.Model.ListDTO;
using System.Text.Json;

namespace BakeryConsoleApp.Logic
{
    internal class Bread
    {
        static MethodApi<BakeryOfficeList> _method;
        public Bread()
        {
            _method = new MethodApi<BakeryOfficeList>();
        }

        public async Task<List<BreadList>?> CheckBread(string bakeryOfficeName)
        {
            var result = await _method.GetAll("bread/bakery-office?bakeryOffice=" + bakeryOfficeName);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<BreadList>>(result, options);
        }

        public async Task<List<BreadList>?> WriteListBreads(List<BakeryOfficeList> bakeryOffice, int option)
        {
            var bakeryOfficeName = bakeryOffice[option].Name;
            var listBreads = await CheckBread(bakeryOfficeName);
            Console.WriteLine("\nPerfect!\n");
            Console.WriteLine("List of Breads:{0}", bakeryOfficeName);
            Console.WriteLine("| {0, -3} | {1, -15} | {2, -5} |", "ID", "NAME", "PRICE");
            Console.WriteLine("|-----|-----------------|-------|");
            foreach (var bread in listBreads)
            {
                Console.WriteLine("| {0, -3} | {1, -15} | {2, -5} |",
                    bread.Id, bread.Name, bread.Price);
            }
            Console.WriteLine();
            return listBreads;
        }
    }
}
