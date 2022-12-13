using BakeryConsoleApp.UtilsApi;
using BakeryFreshBread.Model.ListDTO;
using System.Text.Json;

namespace BakeryConsoleApp.Logic
{
    internal class Bakery
    {
        static MethodApi<BakeryOfficeList> _method;

        public Bakery()
        {
            _method = new MethodApi<BakeryOfficeList>();
        }
        public async Task<List<BakeryOfficeList>?> CheckOffice()
        {
            var result = await _method.GetAll("bakeryOffice");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<BakeryOfficeList>>(result, options);
        }

        public async Task<List<BakeryOfficeList>?> WriteMenuBakery()
        {
            var listBakeries = await CheckOffice();
            if (listBakeries != null)
            {
                Console.WriteLine("\n--------- Menu Bakery Fresh Bread ---------\n");
                Console.WriteLine("Select the office to make an order:");
                foreach (var bakeryItem in listBakeries)
                {
                    Console.WriteLine("{0} {1}", bakeryItem.Id, bakeryItem.Name);
                }
                Console.Write("\n\nType the office to make orders or type Ctrl + C to exit: ");
            }
            else
            {
                Console.Write("\nSorry there are not office availables");
            }

            return listBakeries;
        }
    }
}
