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

namespace TravelVietnam.Application.Features.Regions.Queries
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, List<RegionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetRegionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<RegionDto>> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "regions:all";

            var cachedRegions = await _cacheService.GetAsync<List<RegionDto>>(cacheKey);
            if (cachedRegions != null)
            {
                return cachedRegions;
            }

            var regions = await _unitOfWork.Repository<Region>().Query()
                .Include(r => r.Provinces)
                .Where(r => !r.IsDeleted)
                .OrderBy(r => r.Name)
                .ToListAsync(cancellationToken);

            var regionDtos = _mapper.Map<List<RegionDto>>(regions);

            await _cacheService.SetAsync(cacheKey, regionDtos, TimeSpan.FromHours(2));

            return regionDtos;
        }
    }
}
