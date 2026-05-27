using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelVietnam.Application.DTOs.Auth;
using TravelVietnam.Application.Features.Auth.Commands;

namespace TravelVietnam.WebApi.Controllers
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            try
            {
                var response = await Mediator.Send(command);
                SetRefreshTokenCookie(response.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<int>> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var id = await Mediator.Send(command);
                return Ok(new { UserId = id, Message = "Đăng ký tài khoản thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] TokenRefreshRequest request)
        {
            try
            {
                var refreshToken = request.RefreshToken;
                if (string.IsNullOrEmpty(refreshToken))
                {
                    refreshToken = Request.Cookies["refreshToken"];
                }

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(new { Message = "Yêu cầu Refresh Token." });
                }

                var command = new RefreshTokenCommand
                {
                    AccessToken = request.AccessToken,
                    RefreshToken = refreshToken
                };

                var response = await Mediator.Send(command);
                SetRefreshTokenCookie(response.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true, // HTTPS only
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
