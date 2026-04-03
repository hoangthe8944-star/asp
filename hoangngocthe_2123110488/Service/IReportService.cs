using hoangngocthe_2123110488.DTOs;

public interface IReportService
{
    Task<IEnumerable<ReportDto>> GetAllReportsAsync();
    Task<ReportDto> CreateReportAsync(CreateReportDto dto);
    Task<bool> UpdateStatusAsync(int id, string status);
}