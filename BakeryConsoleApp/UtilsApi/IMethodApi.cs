namespace BakeryConsoleApp.UtilsApi
{
    internal interface IMethodApi<in T>
    {
        Task<string> GetAll(string URL);
        Task<string> GetById(string URL, int id);
        Task<string> Save(string URL, T data);
        Task<string> Update(int id, T data);
    }
}
