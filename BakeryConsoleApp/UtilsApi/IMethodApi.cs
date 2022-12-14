namespace BakeryConsoleApp.UtilsApi
{
    internal interface IMethodApi<in T>
    {
        Task<string> GetAll(string URL);
        Task<string> GetById(string URL, int id);
        Task<string> Save(string URL, T data);
        //For the implementation of the specific functionality is not necessary pass a data 
        Task<string> Update(string URL);
    }
}
