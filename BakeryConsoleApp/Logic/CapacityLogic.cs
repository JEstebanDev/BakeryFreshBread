using BakeryConsoleApp.UtilsApi.Converter;
using BakeryFreshBread.Model.ListDTO;

namespace BakeryConsoleApp.Logic
{
    internal class CapacityLogic
    {
        private readonly Converter<CapacityList> _converter;
        public CapacityLogic()
        {
            _converter = new Converter<CapacityList>();
        }

        public async Task<CapacityList> GetCapacityByIdBakery(int id)
        {
            var result = await _converter.GetById("capacity/BakeryId", id);
            return result;
        }
    }
}
