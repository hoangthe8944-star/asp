using AutoMapper;
using hoangngocthe_2123110488.Model;
using hoangngocthe_2123110488.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Report, ReportDto>();
        CreateMap<CreateReportDto, Report>();
    }
}