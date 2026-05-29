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

namespace TravelVietnam.Application.Features.Blogs.Queries
{
    public class GetBlogsQuery : IRequest<PaginatedList<BlogDto>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBlogsQueryHandler : IRequestHandler<GetBlogsQuery, PaginatedList<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetBlogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<PaginatedList<BlogDto>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"blogs:list:q_{request.SearchTerm?.ToLower() ?? ""}:pn_{request.PageNumber}:ps_{request.PageSize}";

            var cachedList = await _cacheService.GetAsync<PaginatedList<BlogDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var query = _unitOfWork.Repository<Blog>().Query()
                .Include(b => b.MediaFiles)
                .Where(b => !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(term) || b.Content.ToLower().Contains(term));
            }

            query = query.OrderByDescending(b => b.PublishedAt);

            var paginatedBlogs = await PaginatedList<Blog>.CreateAsync(query, request.PageNumber, request.PageSize);

            var dtoList = _mapper.Map<List<BlogDto>>(paginatedBlogs.Items);
            var result = new PaginatedList<BlogDto>(dtoList, paginatedBlogs.TotalCount, paginatedBlogs.PageNumber, request.PageSize);

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
    }
}
