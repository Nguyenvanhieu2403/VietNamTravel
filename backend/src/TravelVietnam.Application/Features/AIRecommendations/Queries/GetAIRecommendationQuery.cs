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

namespace TravelVietnam.Application.Features.AIRecommendations.Queries
{
    public class GetAIRecommendationQuery : IRequest<AIRecommendationResponse>
    {
        public decimal? Budget { get; set; }
        public string? TravelStyle { get; set; } // Adventure, Nature, Cultural, Luxury, Beach
        public string? GroupType { get; set; }   // Solo, Couple, Family, Backpacking
        public int? Month { get; set; }          // 1 - 12
    }

    public class AIRecommendationResponse
    {
        public string Message { get; set; } = null!;
        public string SuggestedItinerary { get; set; } = null!;
        public List<ProvinceListDto> RecommendedProvinces { get; set; } = new();
    }

    public class GetAIRecommendationQueryHandler : IRequestHandler<GetAIRecommendationQuery, AIRecommendationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAIRecommendationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AIRecommendationResponse> Handle(GetAIRecommendationQuery request, CancellationToken cancellationToken)
        {
            var provinceRepository = _unitOfWork.Repository<Province>();
            
            // Fetch all active provinces with their seasons for filtering
            var query = provinceRepository.Query()
                .Include(p => p.Seasons)
                .Where(p => !p.IsDeleted);

            var provinces = await query.ToListAsync(cancellationToken);
            var recommendedProvinces = new List<Province>();

            // 1. Rules matching
            foreach (var province in provinces)
            {
                bool isMatch = true;

                // Budget matching: Allow a buffer of 20% above budget
                if (request.Budget.HasValue && request.Budget > 0)
                {
                    if (province.AverageBudget > request.Budget.Value * 1.2m)
                    {
                        isMatch = false;
                    }
                }

                // Month (Season) matching
                if (request.Month.HasValue && isMatch)
                {
                    var seasonMonths = province.Seasons.SelectMany(s => 
                        s.Months?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()
                    ).Select(m => int.TryParse(m.Trim(), out var parsed) ? parsed : 0);

                    // If they have explicit seasons, boost or filter
                    if (seasonMonths.Any() && !seasonMonths.Contains(request.Month.Value))
                    {
                        // Let's not strictly discard if months don't match, but prioritize them
                        // Or we can discard to be precise
                    }
                }

                // Style & Group filters
                if (isMatch && !string.IsNullOrWhiteSpace(request.TravelStyle))
                {
                    var style = request.TravelStyle.ToLower();
                    if (style == "adventure" && !IsAdventureProvince(province.Slug)) isMatch = false;
                    if (style == "beach" && !IsBeachProvince(province.Slug)) isMatch = false;
                    if (style == "cultural" && !IsCulturalProvince(province.Slug)) isMatch = false;
                    if (style == "nature" && !IsNatureProvince(province.Slug)) isMatch = false;
                }

                if (isMatch)
                {
                    recommendedProvinces.Add(province);
                }
            }

            // 2. Pick top 3-4 recommendations, fallback to major provinces if empty
            if (!recommendedProvinces.Any())
            {
                recommendedProvinces = provinces.Take(3).ToList();
            }
            else
            {
                recommendedProvinces = recommendedProvinces.Take(4).ToList();
            }

            // 3. Formulate custom response messaging
            string message = "Dựa trên sở thích của bạn, AI của Khám phá Việt Nam gợi ý những điểm đến tuyệt vời sau:";
            string itinerary = "GỢI Ý LỊCH TRÌNH:\n";

            if (request.GroupType?.ToLower() == "couple")
            {
                message = "Dành riêng cho kỳ nghỉ lãng mạn của hai bạn, chúng tôi đề xuất các điểm đến yên bình và thơ mộng nhất Việt Nam:";
                itinerary += "- Ngày 1: Đón hoàng hôn tuyệt đẹp, dùng bữa tối lãng mạn dưới ánh nến tại nhà hàng view thung lũng/biển.\n" +
                             "- Ngày 2: Trải nghiệm chèo sup buổi sáng, check-in các quán cafe acoustic mộc mạc và thư thái dạo bộ.\n" +
                             "- Ngày 3: Tận hưởng liệu trình Spa đôi cao cấp và mua sắm quà lưu niệm địa phương.";
            }
            else if (request.GroupType?.ToLower() == "family")
            {
                message = "Đối với chuyến đi gia đình có trẻ nhỏ và người lớn tuổi, chúng tôi ưu tiên các điểm đến có dịch vụ nghỉ dưỡng cao cấp, di chuyển thuận tiện:";
                itinerary += "- Ngày 1: Nhận phòng resort nghỉ dưỡng, vui chơi nhẹ nhàng quanh khuôn viên và tắm hồ bơi.\n" +
                             "- Ngày 2: Khám phá các khu vui chơi giải trí phức hợp hoặc safari động vật hoang dã.\n" +
                             "- Ngày 3: Trải nghiệm ẩm thực địa phương tinh tế tại nhà hàng gia đình truyền thống.";
            }
            else if (request.GroupType?.ToLower() == "backpacking")
            {
                message = "Cho hành trình phượt tự do, chúng tôi gợi ý các cung đường đèo hùng vĩ, chi phí tiết kiệm và đậm chất trải nghiệm:";
                itinerary += "- Ngày 1: Thuê xe máy chinh phục đèo, tối camping cắm trại ngắm sao trời Tây Bắc/Tây Nguyên.\n" +
                             "- Ngày 2: Trekking xuyên rừng quốc gia hoặc trekking bản làng dân tộc thiểu số.\n" +
                             "- Ngày 3: Thưởng thức cà phê phin vỉa hè và giao lưu văn hóa cùng đồng bào bản địa.";
            }
            else
            {
                itinerary += "- Ngày 1: Khám phá các địa danh lịch sử và bảo tàng trung tâm thành phố.\n" +
                             "- Ngày 2: Trải nghiệm ẩm thực đường phố (Food Tour) và chụp ảnh lưu niệm.\n" +
                             "- Ngày 3: Khám phá khu chợ truyền thống mua đặc sản địa phương về làm quà.";
            }

            return new AIRecommendationResponse
            {
                Message = message,
                SuggestedItinerary = itinerary,
                RecommendedProvinces = _mapper.Map<List<ProvinceListDto>>(recommendedProvinces)
            };
        }

        // Helper categorizations
        private bool IsAdventureProvince(string slug) =>
            new[] { "ha-giang", "lao-cai", "lai-chau", "cao-bang", "yen-bai", "quang-binh" }.Contains(slug);

        private bool IsBeachProvince(string slug) =>
            new[] { "da-nang", "quang-nam", "khanh-hoa", "binh-thuan", "ba-ria-vung-tau", "kien-giang" }.Contains(slug);

        private bool IsCulturalProvince(string slug) =>
            new[] { "ha-noi", "thua-thien-hue", "quang-nam", "bac-ninh", "ninh-binh" }.Contains(slug);

        private bool IsNatureProvince(string slug) =>
            new[] { "lao-cai", "ninh-binh", "ha-giang", "backan", "dak-nong", "lam-dong", "an-giang" }.Contains(slug);
    }
}
