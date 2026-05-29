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

namespace TravelVietnam.Application.Features.Destinations.Queries
{
    public class GetDestinationsByProvinceQuery : IRequest<PaginatedList<DestinationDto>>
    {
        public int ProvinceId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetDestinationsByProvinceQueryHandler : IRequestHandler<GetDestinationsByProvinceQuery, PaginatedList<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetDestinationsByProvinceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<DestinationDto>> Handle(GetDestinationsByProvinceQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"destinations:province:{request.ProvinceId}:pn_{request.PageNumber}:ps_{request.PageSize}";

            var cachedList = await _cacheService.GetAsync<PaginatedList<DestinationDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var query = _unitOfWork.Repository<Destination>().Query()
                .Include(d => d.Province)
                .Include(d => d.Region)
                .Include(d => d.MediaFiles)
                .Where(d => !d.IsDeleted && d.ProvinceId == request.ProvinceId)
                .OrderBy(d => d.Name);

            var paginatedDestinations = await PaginatedList<Destination>.CreateAsync(query, request.PageNumber, request.PageSize);

            var dtoList = _mapper.Map<List<DestinationDto>>(paginatedDestinations.Items);
            var result = new PaginatedList<DestinationDto>(dtoList, paginatedDestinations.TotalCount, paginatedDestinations.PageNumber, request.PageSize);

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
    }
}
