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

namespace TravelVietnam.Application.Features.Blogs.Queries
{
    public class GetLatestBlogsQuery : IRequest<List<BlogDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class GetLatestBlogsQueryHandler : IRequestHandler<GetLatestBlogsQuery, List<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetLatestBlogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<BlogDto>> Handle(GetLatestBlogsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"blogs:latest:limit_{request.Limit}";

            var cachedList = await _cacheService.GetAsync<List<BlogDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var blogs = await _unitOfWork.Repository<Blog>().Query()
                .Include(b => b.MediaFiles)
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.PublishedAt ?? b.CreatedAt)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<List<BlogDto>>(blogs);

            await _cacheService.SetAsync(cacheKey, dtoList, TimeSpan.FromMinutes(10));

            return dtoList;
        }
    }
}
