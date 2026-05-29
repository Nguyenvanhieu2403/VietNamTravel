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

namespace TravelVietnam.Application.Features.Destinations.Queries
{
    public class GetDestinationByIdQuery : IRequest<DestinationDto?>
    {
        public int Id { get; set; }
    }

    public class GetDestinationByIdQueryHandler : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<DestinationDto?> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"destination:id:{request.Id}";

            var cachedDestination = await _cacheService.GetAsync<DestinationDto>(cacheKey);
            if (cachedDestination != null)
            {
                return cachedDestination;
            }

            var destination = await _unitOfWork.Repository<Destination>().Query()
                .Include(d => d.MediaFiles)
                .Include(d => d.Province)
                .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

            if (destination == null)
            {
                return null;
            }

            var destinationDto = _mapper.Map<DestinationDto>(destination);

            await _cacheService.SetAsync(cacheKey, destinationDto, TimeSpan.FromHours(1));

            return destinationDto;
        }
    }
}
