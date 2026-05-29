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

namespace TravelVietnam.Application.Features.Cultures.Queries
{
    public class GetCultureBySlugQuery : IRequest<CultureDto?>
    {
        public string Slug { get; set; } = null!;
    }

    public class GetCultureBySlugQueryHandler : IRequestHandler<GetCultureBySlugQuery, CultureDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetCultureBySlugQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<CultureDto?> Handle(GetCultureBySlugQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"culture:slug:{request.Slug.ToLower()}";

            var cachedCulture = await _cacheService.GetAsync<CultureDto>(cacheKey);
            if (cachedCulture != null)
            {
                return cachedCulture;
            }

            var culture = await _unitOfWork.Repository<Culture>().Query()
                .Include(c => c.Region)
                .Include(c => c.MediaFiles)
                .FirstOrDefaultAsync(c => c.Slug.ToLower() == request.Slug.ToLower() && !c.IsDeleted, cancellationToken);

            if (culture == null)
            {
                return null;
            }

            var cultureDto = _mapper.Map<CultureDto>(culture);

            await _cacheService.SetAsync(cacheKey, cultureDto, TimeSpan.FromHours(1));

            return cultureDto;
        }
    }
}
