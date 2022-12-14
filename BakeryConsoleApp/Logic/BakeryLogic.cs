using BakeryFreshBread.Model.ListDTO;
using BakeryConsoleApp.UtilsApi.Converter;

namespace BakeryConsoleApp.Logic
{
    internal class BakeryLogic
    {
        private readonly Converter<BakeryOfficeList> _converter;
        public BakeryLogic()
        {
            _converter = new Converter<BakeryOfficeList>();
        }

        public async Task<List<BakeryOfficeList>?> WriteMenuBakery()
        {

            var listBakeries = await _converter.GetAll("bakeryOffice");
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
