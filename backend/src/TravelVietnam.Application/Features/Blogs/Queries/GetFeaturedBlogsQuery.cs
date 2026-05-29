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
    public class GetFeaturedBlogsQuery : IRequest<List<BlogDto>>
    {
        public int Limit { get; set; } = 10;
    }

    public class GetFeaturedBlogsQueryHandler : IRequestHandler<GetFeaturedBlogsQuery, List<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetFeaturedBlogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<List<BlogDto>> Handle(GetFeaturedBlogsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"blogs:featured:limit_{request.Limit}";

            var cachedList = await _cacheService.GetAsync<List<BlogDto>>(cacheKey);
            if (cachedList != null)
            {
                return cachedList;
            }

            var blogs = await _unitOfWork.Repository<Blog>().Query()
                .Include(b => b.MediaFiles)
                .Where(b => !b.IsDeleted && b.IsFeatured)
                .OrderByDescending(b => b.PublishedAt)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dtoList = _mapper.Map<List<BlogDto>>(blogs);

            await _cacheService.SetAsync(cacheKey, dtoList, TimeSpan.FromHours(1));

            return dtoList;
        }
    }
}
