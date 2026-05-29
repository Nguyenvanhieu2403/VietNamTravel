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

namespace TravelVietnam.Application.Features.Regions.Queries
{
    public class GetRegionBySlugQueryHandler : IRequestHandler<GetRegionBySlugQuery, RegionDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetRegionBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<RegionDto?> Handle(GetRegionBySlugQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"region:slug:{request.Slug.ToLower()}";

            var cachedRegion = await _cacheService.GetAsync<RegionDto>(cacheKey);
            if (cachedRegion != null)
            {
                return cachedRegion;
            }

            var region = await _unitOfWork.Repository<Region>().Query()
                .Include(r => r.Provinces)
                .FirstOrDefaultAsync(r => r.Slug.ToLower() == request.Slug.ToLower() && !r.IsDeleted, cancellationToken);

            if (region == null)
            {
                return null;
            }

            var regionDto = _mapper.Map<RegionDto>(region);

            await _cacheService.SetAsync(cacheKey, regionDto, TimeSpan.FromHours(1));

            return regionDto;
        }
    }
}
