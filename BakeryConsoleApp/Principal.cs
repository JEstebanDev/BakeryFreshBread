using BakeryConsoleApp.Logic;
using BakeryFreshBread.Model.ListDTO;

namespace BakeryConsoleApp
{
    public class Principal
    {

        public static Task Main(string[] args)
        {
            while (true)
            {
                MenuAsync();
            }
        }

        private static void MenuAsync()
        {
            var bakery = new Bakery();
            var listBakery = bakery.WriteMenuBakery().Result;
            var optionOffice = Convert.ToInt32(Console.ReadLine());

            var order = new Order();
            var optionOrder = 0;
            while (optionOrder < 3)
            {
                order.WriteMenuOrder();
                optionOrder = Convert.ToInt32(Console.ReadLine());
                if (optionOrder == 1)
                {
                    var bread = new Bread();
                    var listBread = bread.WriteListBreads(listBakery!, optionOffice).Result;
                    order.WriteAddOrder(listBread, optionOffice);
                }
            }

        }
    }
}