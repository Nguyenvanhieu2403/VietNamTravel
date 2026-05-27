using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Provinces.Queries
{
    public class GetProvincesQuery : IRequest<PaginatedList<ProvinceListDto>>
    {
        public int? RegionId { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProvincesQueryHandler : IRequestHandler<GetProvincesQuery, PaginatedList<ProvinceListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetProvincesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<ProvinceListDto>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
        {
            // 1. Generate Cache Key based on parameters
            var cacheKey = $"provinces:list:r_{request.RegionId ?? 0}:q_{request.SearchTerm?.ToLower() ?? ""}:p_{request.PageNumber}:s_{request.PageSize}";

            // 2. Try fetching from Redis cache
            var cachedList = await _cacheService.GetAsync<PaginatedList<ProvinceListDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            // 3. Cache miss, query database
            var query = _unitOfWork.Repository<Province>().Query().Where(p => !p.IsDeleted);

            // Filtering by region
            if (request.RegionId.HasValue)
            {
                query = query.Where(p => p.RegionId == request.RegionId.Value);
            }

            // Filtering by search term
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term) || (p.Description != null && p.Description.ToLower().Contains(term)));
            }

            // Sorting by Name
            query = query.OrderBy(p => p.Name);

            // 4. Perform pagination
            var paginatedProvinces = await PaginatedList<Province>.CreateAsync(query, request.PageNumber, request.PageSize);

            // 5. Map entities to list DTOs
            var dtoList = _mapper.Map<List<ProvinceListDto>>(paginatedProvinces.Items);
            var result = new PaginatedList<ProvinceListDto>(dtoList, paginatedProvinces.TotalCount, paginatedProvinces.PageNumber, request.PageSize);

            // 6. Save to cache for 10 minutes (shorter lifespan for list queries)
            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
    }
}
