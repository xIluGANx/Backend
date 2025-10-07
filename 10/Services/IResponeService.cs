namespace ResponseDemo.Services
{
    public interface IResponseService
    {
        Task<byte[]> GenerateSampleImageAsync();
        Task<string> GenerateCsvDataAsync();
        Task<Stream> GenerateStreamDataAsync();
    }
}