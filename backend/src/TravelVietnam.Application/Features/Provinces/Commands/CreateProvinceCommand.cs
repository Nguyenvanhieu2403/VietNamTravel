using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Provinces.Commands
{
    public class CreateProvinceCommand : IRequest<int>
    {
        public int RegionId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? CultureDescription { get; set; }
        public string? BestTimeToVisit { get; set; }
        public decimal AverageBudget { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
    }

    public class CreateProvinceCommandHandler : IRequestHandler<CreateProvinceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CreateProvinceCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<int> Handle(CreateProvinceCommand request, CancellationToken cancellationToken)
        {
            var province = new Province
            {
                RegionId = request.RegionId,
                Name = request.Name,
                Slug = request.Slug.ToLower(),
                Description = request.Description,
                CultureDescription = request.CultureDescription,
                BestTimeToVisit = request.BestTimeToVisit,
                AverageBudget = request.AverageBudget,
                VideoUrl = request.VideoUrl,
                ThumbnailUrl = request.ThumbnailUrl
            };

            await _unitOfWork.Repository<Province>().AddAsync(province);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Invalidate Caches (remove all list queries to ensure the new province appears)
            await _cacheService.RemoveByPrefixAsync("provinces:list");

            return province.Id;
        }
    }
}
