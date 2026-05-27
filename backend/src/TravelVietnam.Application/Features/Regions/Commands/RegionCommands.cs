using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Exceptions;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Regions.Commands;

public class CreateRegionCommand : IRequest<RegionDto>
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
}

public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, RegionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public CreateRegionCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<RegionDto> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        var existingRegion = await _unitOfWork.Repository<Region>().Query()
            .FirstOrDefaultAsync(r => r.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (existingRegion != null)
            throw new ConflictException($"Region with slug '{request.Slug}' already exists");

        var region = new Region
        {
            Name = request.Name,
            Slug = request.Slug.ToLower(),
            Description = request.Description
        };

        await _unitOfWork.Repository<Region>().AddAsync(region);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("regions");

        return _mapper.Map<RegionDto>(region);
    }
}

public class UpdateRegionCommand : IRequest<RegionDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, RegionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateRegionCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<RegionDto> Handle(UpdateRegionCommand request, CancellationToken cancellationToken)
    {
        var region = await _unitOfWork.Repository<Region>().Query()
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (region == null)
            throw new NotFoundException($"Region with ID {request.Id} not found");

        var slugExists = await _unitOfWork.Repository<Region>().Query()
            .AnyAsync(r => r.Id != request.Id && r.Slug.ToLower() == request.Slug.ToLower(), cancellationToken);

        if (slugExists)
            throw new ConflictException($"Region with slug '{request.Slug}' already exists");

        region.Name = request.Name;
        region.Slug = request.Slug.ToLower();
        region.Description = request.Description;

        _unitOfWork.Repository<Region>().Update(region);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("regions");

        return _mapper.Map<RegionDto>(region);
    }
}

public class DeleteRegionCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteRegionCommandHandler : IRequestHandler<DeleteRegionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteRegionCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        var region = await _unitOfWork.Repository<Region>().Query()
            .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, cancellationToken);

        if (region == null)
            throw new NotFoundException($"Region with ID {request.Id} not found");

        _unitOfWork.Repository<Region>().Delete(region);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveByPrefixAsync("regions");

        return true;
    }
}
