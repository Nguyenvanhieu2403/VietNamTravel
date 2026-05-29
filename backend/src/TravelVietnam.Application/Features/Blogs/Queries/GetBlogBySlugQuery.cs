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

namespace TravelVietnam.Application.Features.Blogs.Queries
{
    public class GetBlogBySlugQuery : IRequest<BlogDto?>
    {
        public string Slug { get; set; } = null!;
    }

    public class GetBlogBySlugQueryHandler : IRequestHandler<GetBlogBySlugQuery, BlogDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetBlogBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<BlogDto?> Handle(GetBlogBySlugQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"blog:slug:{request.Slug.ToLower()}";

            var cachedBlog = await _cacheService.GetAsync<BlogDto>(cacheKey);
            if (cachedBlog != null)
            {
                return cachedBlog;
            }

            var blog = await _unitOfWork.Repository<Blog>().Query()
                .Include(b => b.MediaFiles)
                .FirstOrDefaultAsync(b => b.Slug.ToLower() == request.Slug.ToLower() && !b.IsDeleted, cancellationToken);

            if (blog == null)
            {
                return null;
            }

            var blogDto = _mapper.Map<BlogDto>(blog);

            await _cacheService.SetAsync(cacheKey, blogDto, TimeSpan.FromHours(1));

            return blogDto;
        }
    }
}
