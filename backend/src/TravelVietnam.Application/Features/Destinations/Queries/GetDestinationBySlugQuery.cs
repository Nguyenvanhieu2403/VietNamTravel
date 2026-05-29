using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Destinations.Queries
{
    public class GetDestinationBySlugQuery : IRequest<DestinationDto?>
    {
        public string Slug { get; set; } = null!;
    }

    public class GetDestinationBySlugQueryHandler : IRequestHandler<GetDestinationBySlugQuery, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetDestinationBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<DestinationDto?> Handle(GetDestinationBySlugQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"destination:slug:{request.Slug.ToLower()}";

            var cachedDestination = await _cacheService.GetAsync<DestinationDto>(cacheKey);
            if (cachedDestination != null)
            {
                return cachedDestination;
            }

            var destination = await _unitOfWork.Repository<Destination>().Query()
                .Include(d => d.MediaFiles)
                .Include(d => d.Province)
                .Include(d => d.Region)
                .FirstOrDefaultAsync(d => d.Slug.ToLower() == request.Slug.ToLower() && !d.IsDeleted, cancellationToken);

            if (destination == null)
            {
                return null;
            }

            var destinationDto = _mapper.Map<DestinationDto>(destination);

            await _cacheService.SetAsync(cacheKey, destinationDto, TimeSpan.FromHours(1));

            return destinationDto;
        }
    }
}
