using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelVietnam.Application.Common.Security;
using TravelVietnam.Application.DTOs.Auth;
using TravelVietnam.Application.Interfaces;
using TravelVietnam.Domain.Entities;

namespace TravelVietnam.Application.Features.Auth.Commands
{
    // ----------------------------------------------------
    // 1. REGISTER COMMAND
    // ----------------------------------------------------
    public class RegisterCommand : IRequest<int>
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userRepository = _unitOfWork.Repository<User>();
            
            // Check uniqueness
            var existingUser = await userRepository.Query()
                .AnyAsync(u => u.Username.ToLower() == request.Username.ToLower() || u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

            if (existingUser)
            {
                throw new Exception("Tên đăng nhập hoặc Email đã được đăng ký sử dụng.");
            }

            // Fetch default "User" role
            var role = await _unitOfWork.Repository<Role>().Query()
                .FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);
            
            if (role == null)
            {
                throw new Exception("Lỗi hệ thống: Không tìm thấy vai trò người dùng (User Role).");
            }

            var user = new User
            {
                RoleId = role.Id,
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            await userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }

    // ----------------------------------------------------
    // 2. LOGIN COMMAND
    // ----------------------------------------------------
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<User>().Query()
                .Include(u => u.Role)
                    .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower() && !u.IsDeleted, cancellationToken);

            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            // Generate Tokens
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshTokenString = _jwtService.GenerateRefreshToken();

            // Save Refresh Token to Database (HTTP-Only verification)
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenString,
                ExpiresAt = DateTime.UtcNow.AddDays(7) // Refresh token valid for 7 days
            };

            await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role.Name
            };
        }
    }

    // ----------------------------------------------------
    // 3. REFRESH TOKEN COMMAND
    // ----------------------------------------------------
    public class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenEntity = await _unitOfWork.Repository<RefreshToken>().Query()
                .Include(rt => rt.User)
                    .ThenInclude(u => u.Role)
                        .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (tokenEntity == null || !tokenEntity.IsActive)
            {
                throw new Exception("Refresh Token không hợp lệ hoặc đã hết hạn.");
            }

            // Revoke current token
            tokenEntity.RevokedAt = DateTime.UtcNow;
            _unitOfWork.Repository<RefreshToken>().Update(tokenEntity);

            // Generate new pair
            var user = tokenEntity.User;
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshTokenString = _jwtService.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshTokenString,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.Repository<RefreshToken>().AddAsync(newRefreshTokenEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenString,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role.Name
            };
        }
    }
}
