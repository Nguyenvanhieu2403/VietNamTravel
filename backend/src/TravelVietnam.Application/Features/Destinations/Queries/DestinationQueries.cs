using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Exceptions;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Destinations.Queries;

public class GetDestinationsQuery : IRequest<PaginatedResponse<DestinationDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int? ProvinceId { get; set; }
    public string? SearchTerm { get; set; }
}

public class GetDestinationsQueryHandler : IRequestHandler<GetDestinationsQuery, PaginatedResponse<DestinationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetDestinationsQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<DestinationDto>> Handle(GetDestinationsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"destinations:list:page:{request.PageNumber}:province:{request.ProvinceId}:search:{request.SearchTerm}";
        var cachedResult = await _cacheService.GetAsync<PaginatedResponse<DestinationDto>>(cacheKey);
        if (cachedResult != null)
            return cachedResult;

        var query = _unitOfWork.Repository<Destination>().Query()
            .Where(d => !d.IsDeleted);

        if (request.ProvinceId.HasValue)
            query = query.Where(d => d.ProvinceId == request.ProvinceId);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(d => d.Name.ToLower().Contains(request.SearchTerm.ToLower()));

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(d => d.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<DestinationDto>>(items);
        var result = new PaginatedResponse<DestinationDto>
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

public class GetDestinationByIdQuery : IRequest<DestinationDto>
{
    public int Id { get; set; }
}

public class GetDestinationByIdQueryHandler : IRequestHandler<GetDestinationByIdQuery, DestinationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<DestinationDto> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"destination:id:{request.Id}";
        var cachedResult = await _cacheService.GetAsync<DestinationDto>(cacheKey);
        if (cachedResult != null)
            return cachedResult;

        var destination = await _unitOfWork.Repository<Destination>().Query()
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (destination == null)
            throw new NotFoundException(nameof(Destination), request.Id);

        var dto = _mapper.Map<DestinationDto>(destination);
        await _cacheService.SetAsync(cacheKey, dto, TimeSpan.FromHours(1));
        return dto;
    }
}

public class GetRelatedDestinationsQuery : IRequest<List<DestinationDto>>
{
    public int DestinationId { get; set; }
    public int Limit { get; set; } = 5;
}

public class GetRelatedDestinationsQueryHandler : IRequestHandler<GetRelatedDestinationsQuery, List<DestinationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public GetRelatedDestinationsQueryHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<List<DestinationDto>> Handle(GetRelatedDestinationsQuery request, CancellationToken cancellationToken)
    {
        var destination = await _unitOfWork.Repository<Destination>().Query()
            .FirstOrDefaultAsync(d => d.Id == request.DestinationId && !d.IsDeleted, cancellationToken);

        if (destination == null)
            throw new NotFoundException(nameof(Destination), request.DestinationId);

        var related = await _unitOfWork.Repository<Destination>().Query()
            .Where(d => d.ProvinceId == destination.ProvinceId && d.Id != request.DestinationId && !d.IsDeleted)
            .OrderBy(d => d.Name)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DestinationDto>>(related);
    }
}
