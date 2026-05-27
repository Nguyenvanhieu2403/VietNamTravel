using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Exceptions;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Regions.Queries;

public class GetRegionsQuery : IRequest<PaginatedResponse<RegionDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchTerm { get; set; }
}

public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, PaginatedResponse<RegionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetRegionsQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RegionDto>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"regions:list:page:{request.PageNumber}:pagesize:{request.PageSize}:search:{request.SearchTerm ?? ""}";
        var cachedResult = await _cacheService.GetAsync<PaginatedResponse<RegionDto>>(cacheKey);
        if (cachedResult != null)
            return cachedResult;

        var query = _unitOfWork.Repository<Region>().Query()
            .Include(r => r.Provinces)
            .Where(r => !r.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(searchLower) || r.Description!.ToLower().Contains(searchLower));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(r => r.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<RegionDto>>(items);
        var result = new PaginatedResponse<RegionDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }
}

public class GetRegionByIdQuery : IRequest<RegionDto>
{
    public int Id { get; set; }
}

public class GetRegionByIdQueryHandler : IRequestHandler<GetRegionByIdQuery, RegionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetRegionByIdQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<RegionDto> Handle(GetRegionByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"region:id:{request.Id}";
        var cachedResult = await _cacheService.GetAsync<RegionDto>(cacheKey);
        if (cachedResult != null)
            return cachedResult;

        var region = await _unitOfWork.Repository<Region>().Query()
            .Include(r => r.Provinces)
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (region == null)
            throw new NotFoundException(nameof(Region), request.Id);

        var dto = _mapper.Map<RegionDto>(region);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));
        return dto;
    }
}

public class GetRegionBySlugQuery : IRequest<RegionDto>
{
    public string Slug { get; set; } = null!;
}

public class GetRegionBySlugQueryHandler : IRequestHandler<GetRegionBySlugQuery, RegionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetRegionBySlugQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<RegionDto> Handle(GetRegionBySlugQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"region:slug:{request.Slug}";
        var cachedResult = await _cacheService.GetAsync<RegionDto>(cacheKey);
        if (cachedResult != null)
            return cachedResult;

        var region = await _unitOfWork.Repository<Region>().Query()
            .Include(r => r.Provinces)
            .FirstOrDefaultAsync(r => r.Slug.ToLower() == request.Slug.ToLower() && !r.IsDeleted, cancellationToken);

        if (region == null)
            throw new NotFoundException($"Region with slug '{request.Slug}' not found");

        var dto = _mapper.Map<RegionDto>(region);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));
        return dto;
    }
}
