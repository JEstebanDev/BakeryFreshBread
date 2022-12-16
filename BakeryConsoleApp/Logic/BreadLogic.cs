using BakeryConsoleApp.UtilsApi.Converter;
using BakeryFreshBread.Model.ListDTO;

namespace BakeryConsoleApp.Logic
{
    internal class BreadLogic
    {
        private readonly Converter<BreadList> _converter;
        public BreadLogic()
        {
            _converter = new Converter<BreadList>();
        }
        public async Task<List<BreadList>?> WriteListBreads(string bakeryOfficeName)
        {
            var listBreads = await _converter.GetAll("bread/bakery-office?bakeryOffice=" + bakeryOfficeName);
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

        public async Task<BreadList> GetAllBreads(int breadId)
        {
            return await _converter.GetById("bread", breadId);
        }
    }
}
