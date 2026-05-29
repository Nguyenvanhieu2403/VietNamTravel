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

namespace TravelVietnam.Application.Features.Cultures.Queries
{
    public class GetFeaturedCulturesQuery : IRequest<List<CultureDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class GetFeaturedCulturesQueryHandler : IRequestHandler<GetFeaturedCulturesQuery, List<CultureDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetFeaturedCulturesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<CultureDto>> Handle(GetFeaturedCulturesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"cultures:featured:limit_{request.Limit}";

            var cachedList = await _cacheService.GetAsync<List<CultureDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var cultures = await _unitOfWork.Repository<Culture>().Query()
                .Include(c => c.Region)
                .Include(c => c.MediaFiles)
                .Where(c => !c.IsDeleted && c.IsFeatured)
                .OrderByDescending(c => c.CreatedAt)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<List<CultureDto>>(cultures);

            await _cacheService.SetAsync(cacheKey, dtoList, TimeSpan.FromHours(1));

            return dtoList;
        }
    }
}
