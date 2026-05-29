using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Models;
using TravelVietnam.Application.DTOs.Travel;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Cultures.Queries
{
    public class GetCulturesByRegionQuery : IRequest<PaginatedList<CultureDto>>
    {
        public int RegionId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCulturesByRegionQueryHandler : IRequestHandler<GetCulturesByRegionQuery, PaginatedList<CultureDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetCulturesByRegionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<CultureDto>> Handle(GetCulturesByRegionQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"cultures:region:{request.RegionId}:pn_{request.PageNumber}:ps_{request.PageSize}";

            var cachedList = await _cacheService.GetAsync<PaginatedList<CultureDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var query = _unitOfWork.Repository<Culture>().Query()
                .Include(c => c.Region)
                .Include(c => c.MediaFiles)
                .Where(c => !c.IsDeleted && c.RegionId == request.RegionId)
                .OrderByDescending(c => c.CreatedAt);

            var paginatedCultures = await PaginatedList<Culture>.CreateAsync(query, request.PageNumber, request.PageSize);

            var dtoList = _mapper.Map<List<CultureDto>>(paginatedCultures.Items);
            var result = new PaginatedList<CultureDto>(dtoList, paginatedCultures.TotalCount, paginatedCultures.PageNumber, request.PageSize);

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
