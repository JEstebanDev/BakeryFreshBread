using BakeryFreshBread.Model.Enum;

namespace BakeryConsoleApp.UtilsApi.Converter
{
    internal interface IConverter<T>
    {
        Task<List<T>> GetAll(string URL);
        Task<T> GetById(string URL, int id);
        Task<T> Save(string URL, T data);
        //For the implementation of the specific functionality is not necessary pass a data 
        Task<T> UpdateOrderStatus(int id, Status status);
    }
}
