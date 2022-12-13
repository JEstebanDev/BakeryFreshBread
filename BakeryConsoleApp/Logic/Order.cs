using BakeryConsoleApp.UtilsApi;
using BakeryFreshBread.Model.DTO;
using BakeryFreshBread.Model.ListDTO;
using System.Text.Json;
using BakeryFreshBread.Model.Enum;

namespace BakeryConsoleApp.Logic
{
    public class Order
    {
        static MethodApi<OrderDTO> _method;
        public Order()
        {
            _method = new MethodApi<OrderDTO>();
        }

        public async Task<List<OrderList>?> CheckOrders()
        {
            var result = await _method.GetAll("order");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<OrderList>>(result, options);
        }

        public async Task<OrderDTO> PostOrder(OrderDTO data)
        {
            var result = await _method.Save("order", data);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<OrderDTO>(result, options);
        }

        public async Task<OrderDTO> GetOrderById(int idOrder)
        {
            var result = await _method.GetById("order", idOrder);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<OrderDTO>(result, options);
        }


        public async void WriteMenuOrder()
        {
            var listOrder = await CheckOrders();
            Console.WriteLine("\nPerfect!\n");
            Console.WriteLine("Select an action:");
            Console.WriteLine("1: Add order");
            if (listOrder != null)
            {
                Console.WriteLine("2: Prepare all the orders");
            }
            Console.WriteLine("3: Go back");
            Console.Write("\n\nType an action or type Ctrl + C to exit: ");
        }

        public async void WriteAddOrder(List<BreadList> listBread, int optionOffice)
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
            var capacity = new Capacity();
            var resultApiCapacity = capacity.CheckCapacity(optionOffice).Result;
            Console.WriteLine(resultApiCapacity.CapacityOffice);
            var value = resultApiCapacity.CapacityOffice - totalQuantity;
            if (value >= 0)
            {
                Console.WriteLine("The Order has a cost of ${0}, Do you want to generate the order? (yes)/(no)", totalPrice);
                var option = Console.ReadLine();
                if (option.Equals("yes"))
                {
                    CreateOrder(breadOrder);
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
            var totalQuantity = 0;
            foreach (var order in breadOrder)
            {
                totalQuantity += order.Quantity;
            }
            return totalQuantity;
        }

        private async void CreateOrder(ICollection<BreadOrderDTO> breadOrder)
        {
            var dto = new OrderDTO
            {
                Status = Status.Pending,
                TotalPrice = 1,
                BreadOrder = breadOrder
            };
            var isSaved = await PostOrder(dto);
            if (isSaved != null)
            {
                Console.WriteLine("Order successfully created with a total cost of: ${0}", isSaved.TotalPrice);
            }
        }
    }
}
