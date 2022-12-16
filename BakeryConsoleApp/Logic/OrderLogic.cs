using BakeryFreshBread.Model.DTO;
using BakeryFreshBread.Model.ListDTO;
using BakeryFreshBread.Model.Enum;
using BakeryConsoleApp.UtilsApi.Converter;

namespace BakeryConsoleApp.Logic
{
    public class OrderLogic
    {
        private readonly Converter<OrderList> _converter;
        private readonly Converter<OrderDTO> _converter2;
        private readonly Converter<bool> _converter3;
        private readonly Converter<IngredientList> _converter4;
        public OrderLogic()
        {
            _converter = new Converter<OrderList>();
            _converter2 = new Converter<OrderDTO>();
            _converter3 = new Converter<bool>();
        }

        public async Task<List<OrderList>> ProfitsOffices(string bakeryOfficeName)
        {
            return await _converter.GetAll("office-status?office=" + bakeryOfficeName + "&status=1");
        }

        public async Task<bool> WriteMenuOrder(string bakeryOfficeName)
        {
            var cont = 1;
            var listOrder = await _converter.GetAll("office-status?office=" + bakeryOfficeName + "&status=0");
            Console.WriteLine("\nPerfect!\n");
            Console.WriteLine("Select an action:");
            Console.WriteLine("{0}: Add order", cont);
            cont++;
            if (listOrder.Count != 0)
            {
                Console.WriteLine("{0}: Prepare all the orders", cont);
                cont++;
            }
            Console.WriteLine("{0}: Go back", cont);
            Console.Write("\n\nType an action or type Ctrl + C to exit: ");
            return listOrder.Count != 0;
        }

        public void WriteAddOrder(List<BreadList> listBread, int optionOffice)
        {
            Console.WriteLine("\nHow many breads will you add to the order?");
            var quantityBreads = Convert.ToInt32(Console.ReadLine());
            ICollection<BreadOrderDTO> breadOrder = new List<BreadOrderDTO>();
            for (var i = 0; i < quantityBreads; i++)
            {
                Console.WriteLine("Add the bread and the quantity with this format [idBread,quantity]");
                var format = Console.ReadLine();
                var orders = format.Split(",");
                breadOrder.Add(new BreadOrderDTO()
                {
                    IdBread = Convert.ToInt32(orders[0]),
                    Quantity = Convert.ToInt32(orders[1])
                });
            }
            var totalPrice = TotalPrice(listBread, breadOrder);
            var totalQuantity = TotalQuantity(breadOrder);
            var capacity = new CapacityLogic();
            var resultApiCapacity = capacity.GetCapacityByIdBakery(optionOffice).Result;
            var value = resultApiCapacity.CapacityOffice - totalQuantity;
            if (value >= 0)
            {
                Console.WriteLine("The Order has a cost of ${0}, Do you want to generate the order? (yes)/(no)", totalPrice);
                var option = Console.ReadLine();
                if (option.Equals("yes"))
                {
                    CreateOrder(breadOrder);
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("Sorry this office does not have the capacity to process this order");
                Console.WriteLine("The capacity available is: {0} breads", resultApiCapacity.CapacityOffice);
            }
        }
        private int TotalPrice(List<BreadList> listBread, ICollection<BreadOrderDTO> breadOrder)
        {
            var totalPrice = 0;
            foreach (var order in breadOrder)
            {
                var breadValue = listBread.Find(x => x.Id == order.IdBread);
                if (breadValue != null)
                {
                    totalPrice += breadValue.Price * order.Quantity;
                }
            }
            return totalPrice;
        }
        private int TotalQuantity(ICollection<BreadOrderDTO> breadOrder)
        {
            return breadOrder.Sum(order => order.Quantity);
        }

        private async void CreateOrder(ICollection<BreadOrderDTO> breadOrder)
        {
            var dto = new OrderDTO
            {
                Status = Status.Pending,
                TotalPrice = 1,
                BreadOrder = breadOrder
            };
            var isSaved = await _converter2.Save("order", dto);
            if (isSaved != null)
            {
                Console.WriteLine("Order successfully created with a total cost of: ${0}", isSaved.TotalPrice);
            }
            else
            {
                Console.WriteLine("Error creating the order, please try again!");
            }
        }

        public async Task<bool> PrepareOrders(string bakeryOfficeName)
        {
            Console.WriteLine("Are you sure? (yes)/(no)");
            var option = Console.ReadLine();
            if (option.Equals("yes"))
            {
                var listOrder = await _converter.GetAll("office-status?office=" + bakeryOfficeName + "&status=0");
                if (listOrder.Count != 0)
                {
                    foreach (var order in listOrder)
                    {
                        var isUpdated = _converter3.UpdateOrderStatus(order.Id).Result;
                        if (isUpdated == false)
                        {
                            Console.WriteLine("Error Updating the orders");
                            break;
                        }
                    }
                    foreach (var order in listOrder)
                    {
                        var breadList = await GetBreads(order.BreadOrder);
                        ShowPreparation(breadList);
                    }
                }
            }

            return true;
        }

        private void ShowPreparation(List<BreadList> breadList)
        {
            Console.WriteLine("\n--------- Preparing order ---------\n");
            foreach (var bread in breadList)
            {
                Console.WriteLine("Bread: {0}", bread.Name);
                Console.WriteLine("INGREDIENTS");
                var ingredientList = GetIngredientName(bread.Ingredient).Result;

                foreach (var step in bread.Step)
                {
                    var stepMix = step.Name.Contains("Mixing");
                    var stepRest = step.Name.Contains("rest");
                    var stepFerment = step.Name.Contains("ferment");
                    var stepFold = step.Name.Contains("Fold");
                    var stepCook = step.Name.Contains("Cook");
                    if (stepMix)
                    {
                        Console.Write(step.Name);
                        MixingMessage(ingredientList);
                    }
                    if (stepRest)
                    {
                        Console.WriteLine("{0} {1}", step.Name, bread.RestingTime);
                    }
                    if (stepFerment)
                    {
                        Console.WriteLine("{0} {1}", step.Name, bread.FermentTime);
                    }
                    if (stepFold)
                    {
                        Console.WriteLine(step.Name);
                    }
                    if (stepCook)
                    {
                        Console.WriteLine("{0} {1} at {2}", step.Name, bread.CookingTime, bread.CookingTemperature);
                    }
                }
                Console.WriteLine("------------------------------------");
            }
        }

        private static void MixingMessage(List<BreadIngredientList> ingredientList)
        {
            foreach (var ingredient in ingredientList)
            {
                Console.Write("{0} of {1}, ", ingredient.Name, ingredient.Quantity);
            }
        }

        private async Task<List<BreadIngredientList>> GetIngredientName(ICollection<BreadIngredientDTO> breadIngredient)
        {
            var ingredientList = new List<BreadIngredientList>();
            foreach (var ingredient in breadIngredient)
            {
                var ingredientLogic = new IngredientLogic();
                var ingredientValue = await ingredientLogic.GetIngredientById(ingredient.IdIngredient);
                Console.WriteLine("{0} - {1}", ingredientValue.Name, ingredient.Quantity);
                ingredientList.Add(new BreadIngredientList()
                {
                    Name = ingredientValue.Name,
                    Quantity = ingredient.Quantity
                });
            }
            return ingredientList;
        }


        private async Task<List<BreadList>> GetBreads(ICollection<BreadOrderDTO> orderBreadOrder)
        {
            var breadsList = new List<BreadList>();
            foreach (var bread in orderBreadOrder)
            {
                var breadLogic = new BreadLogic();
                var breadValue = await breadLogic.GetAllBreads(bread.IdBread);
                breadsList.Add(breadValue);
            }
            return breadsList;
        }
    }
}
