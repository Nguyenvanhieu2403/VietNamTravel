using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Provinces.Queries
{
    public class GetProvinceBySlugQuery : IRequest<ProvinceDto?>
    {
        public string Slug { get; set; } = null!;
    }

    public class GetProvinceBySlugQueryHandler : IRequestHandler<GetProvinceBySlugQuery, ProvinceDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetProvinceBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ProvinceDto?> Handle(GetProvinceBySlugQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"province:slug:{request.Slug.ToLower()}";
            
            // 1. Try reading from cache
            var cachedProvince = await _cacheService.GetAsync<ProvinceDto>(cacheKey);
            if (cachedProvince != null)
            {
                return cachedProvince;
            }

            // 2. Cache miss, query database
            var province = await _unitOfWork.Repository<Province>().Query()
                .Include(p => p.Region)
                .Include(p => p.Destinations)
                    .ThenInclude(d => d.MediaFiles)
                .Include(p => p.Foods)
                .Include(p => p.Festivals)
                .Include(p => p.Seasons)
                .Include(p => p.MediaFiles)
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Slug.ToLower() == request.Slug.ToLower() && !p.IsDeleted, cancellationToken);

            if (province == null)
            {
                return null;
            }

            // 3. Map to DTO
            var provinceDto = _mapper.Map<ProvinceDto>(province);

            // 4. Save to Redis cache for 1 hour
            await _cacheService.SetAsync(cacheKey, provinceDto, TimeSpan.FromHours(1));

            return provinceDto;
        }
    }
}
