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
            if (listBakeries.Count != 0)
            {
                Console.WriteLine("\n--------- Menu  ---------\n");
                Console.WriteLine("Select the office to make an order:");
                CheckTotalOrder(listBakeries);
                Console.Write("\n\nType the office to make orders or type Ctrl + C to exit: ");
            }
            else
            {
                Console.Write("\nSorry there are not office availables");
            }

            return listBakeries;
        }

        private static void CheckTotalOrder(List<BakeryOfficeList> bakeryOfficeLists)
        {
            Console.WriteLine("| {0, -3} | {1, -15} | {2, -8}| {3, -7} |", "ID", "BAKERYNAME", "ORDER", "PROFITS");
            Console.WriteLine("|-----|-----------------|---------|---------|");
            var orderLogic = new OrderLogic();
            foreach (var bakery in bakeryOfficeLists)
            {
                var listProfits = orderLogic.ProfitsOffices(bakery.Name).Result;
                var profits = 0;
                var orders = 0;
                foreach (var orderList in listProfits)
                {
                    profits += orderList.TotalPrice;
                    orders++;
                }
                Console.WriteLine("| {0, -3} | {1, -15} | {2, -7} | {3, -7} |",
                    bakery.Id, bakery.Name, orders, profits);
            }
        }
    }
}
