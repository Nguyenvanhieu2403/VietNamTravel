using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
