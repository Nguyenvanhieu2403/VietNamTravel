using System;
using System.Collections.Generic;
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
    public class GetFeaturedDestinationsQuery : IRequest<List<DestinationDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class GetFeaturedDestinationsQueryHandler : IRequestHandler<GetFeaturedDestinationsQuery, List<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetFeaturedDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<DestinationDto>> Handle(GetFeaturedDestinationsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"destinations:featured:limit_{request.Limit}";

            var cachedList = await _cacheService.GetAsync<List<DestinationDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var destinations = await _unitOfWork.Repository<Destination>().Query()
                .Include(d => d.Province)
                .Include(d => d.Region)
                .Include(d => d.MediaFiles)
                .Where(d => !d.IsDeleted && d.IsFeatured)
                .OrderByDescending(d => d.Rating)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<List<DestinationDto>>(destinations);

            await _cacheService.SetAsync(cacheKey, dtoList, TimeSpan.FromHours(1));

            return dtoList;
        }
    }
}
