using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Exceptions;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Destinations.Commands;

// ============ CREATE DESTINATION ============
public class CreateDestinationCommand : IRequest<DestinationDto>
{
    public int ProvinceId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal EntryFee { get; set; }
    public string? ThumbnailUrl { get; set; }
}

public class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, DestinationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public CreateDestinationCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<DestinationDto> Handle(CreateDestinationCommand request, CancellationToken cancellationToken)
    {
        var provinceExists = await _unitOfWork.Repository<Province>().Query()
            .AnyAsync(p => p.Id == request.ProvinceId && !p.IsDeleted, cancellationToken);
        if (!provinceExists)
            throw new NotFoundException(nameof(Province), request.ProvinceId);

        var destination = new Destination
        {
            ProvinceId = request.ProvinceId,
            Name = request.Name,
            Slug = request.Slug.ToLower(),
            Description = request.Description,
            Address = request.Address,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            EntryFee = request.EntryFee,
            ThumbnailUrl = request.ThumbnailUrl
        };

        await _unitOfWork.Repository<Destination>().AddAsync(destination);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveByPrefixAsync("destinations");

        return _mapper.Map<DestinationDto>(destination);
    }
}

// ============ UPDATE DESTINATION ============
public class UpdateDestinationCommand : IRequest<DestinationDto>
{
    public int Id { get; set; }
    public int ProvinceId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal EntryFee { get; set; }
    public string? ThumbnailUrl { get; set; }
}

public class UpdateDestinationCommandHandler : IRequestHandler<UpdateDestinationCommand, DestinationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly AutoMapper.IMapper _mapper;

    public UpdateDestinationCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, AutoMapper.IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<DestinationDto> Handle(UpdateDestinationCommand request, CancellationToken cancellationToken)
    {
        var destination = await _unitOfWork.Repository<Destination>().Query()
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);
        if (destination == null)
            throw new NotFoundException(nameof(Destination), request.Id);

        var provinceExists = await _unitOfWork.Repository<Province>().Query()
            .AnyAsync(p => p.Id == request.ProvinceId && !p.IsDeleted, cancellationToken);
        if (!provinceExists)
            throw new NotFoundException(nameof(Province), request.ProvinceId);

        destination.ProvinceId = request.ProvinceId;
        destination.Name = request.Name;
        destination.Slug = request.Slug.ToLower();
        destination.Description = request.Description;
        destination.Address = request.Address;
        destination.Latitude = request.Latitude;
        destination.Longitude = request.Longitude;
        destination.EntryFee = request.EntryFee;
        destination.ThumbnailUrl = request.ThumbnailUrl;

        _unitOfWork.Repository<Destination>().Update(destination);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveByPrefixAsync("destinations");

        return _mapper.Map<DestinationDto>(destination);
    }
}

// ============ DELETE DESTINATION ============
public class DeleteDestinationCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteDestinationCommandHandler : IRequestHandler<DeleteDestinationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteDestinationCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<bool> Handle(DeleteDestinationCommand request, CancellationToken cancellationToken)
    {
        var destination = await _unitOfWork.Repository<Destination>().Query()
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);
        if (destination == null)
            throw new NotFoundException(nameof(Destination), request.Id);

        _unitOfWork.Repository<Destination>().Delete(destination);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveByPrefixAsync("destinations");

        return true;
    }
}
