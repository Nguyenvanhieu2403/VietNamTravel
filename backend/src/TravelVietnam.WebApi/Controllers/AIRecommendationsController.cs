using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.Features.AIRecommendations.Queries;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class AIRecommendationsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<AIRecommendationResponse>> Get([FromQuery] GetAIRecommendationQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
