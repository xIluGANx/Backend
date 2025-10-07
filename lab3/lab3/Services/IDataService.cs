public interface IDataService
{
    Task<List<string>> GetDataAsync();
    Task<bool> SaveDataAsync(string data);
    Task<string> ProcessDataAsync(string input);
}