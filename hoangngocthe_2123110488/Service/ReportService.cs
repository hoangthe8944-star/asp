using AutoMapper;
using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Model;

public class ReportService : IReportService
{
    private readonly IReportRepository _repo;
    private readonly IMapper _mapper;

    public ReportService(IReportRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
    {
        var reports = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ReportDto>>(reports);
    }

    public async Task<ReportDto> CreateReportAsync(CreateReportDto dto)
    {
        var report = _mapper.Map<Report>(dto);
        report.Status = "pending";
        report.CreatedAt = DateTime.UtcNow;

        await _repo.AddAsync(report);
        await _repo.SaveChangesAsync();
        return _mapper.Map<ReportDto>(report);
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var report = await _repo.GetByIdAsync(id);
        if (report == null) return false;

        report.Status = status;
        await _repo.UpdateAsync(report);
        await _repo.SaveChangesAsync();
        return true;
    }
}