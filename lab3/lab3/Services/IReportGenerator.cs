
public interface IReportGenerator
{
    Task<string> GenerateUserReportAsync(List<User> users);
    Task<string> GenerateDataReportAsync(List<string> data);
    string ReportType { get; }
}