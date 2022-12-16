using BakeryConsoleApp.UtilsApi.Converter;
using BakeryFreshBread.Model.ListDTO;

namespace BakeryConsoleApp.Logic
{
    internal class IngredientLogic
    {
        private readonly Converter<IngredientList> _converter;
        public IngredientLogic()
        {
            _converter = new Converter<IngredientList>();
        }

        public async Task<IngredientList> GetIngredientById(int id)
        {
            var result = await _converter.GetById("ingredient", id);
            return result;
        }
    }
}
