using BakeryConsoleApp.Logic;
using BakeryConsoleApp.Resource;
using System.Diagnostics;

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
            ASCIIART asd = new ASCIIART();
            Console.WriteLine(asd.Bread);
            Console.WriteLine(asd.Bakery);
            var bakery = new BakeryLogic();
            var listBakery = bakery.WriteMenuBakery().Result;
            var optionOffice = Convert.ToInt32(Console.ReadLine());

            var order = new OrderLogic();
            var optionOrder = 0;
            while (optionOrder < 3)
            {
                var bakeryOfficeName = listBakery[optionOffice - 1].Name;
                var existOrders = order.WriteMenuOrder(bakeryOfficeName).Result;
                optionOrder = Convert.ToInt32(Console.ReadLine());
                if (optionOrder == 1)
                {
                    var bread = new BreadLogic();
                    var listBread = bread.WriteListBreads(bakeryOfficeName).Result;
                    order.WriteAddOrder(listBread, optionOffice);
                }
                if (optionOrder == 2)
                {
                    if (existOrders)
                    {
                        var value = order.PrepareOrders(bakeryOfficeName).Result;
                    }
                    else
                    {
                        optionOrder = 3;
                    }
                }
            }
        }
    }
}