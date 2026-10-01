using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillLoop.Application.DTOs.Auth;
using SkillLoop.Application.Interfaces.IService;
using System.Security.Claims;

namespace SkillLoop.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<ActionResult<MessageResponse>> Register(RegisterRequest request)
        {
            var result = await _auth.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
        {
            await _auth.VerifyEmailAsync(request);
            return NoContent();
        }

        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendEmailVerification(ResendEmailVerificationRequest request)
        {
            await _auth.ResendEmailVerificationAsync(request);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var result = await _auth.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("social-login")]
        public async Task<ActionResult<AuthResponse>> SocialLogin(SocialLoginRequest request)
        {
            var result = await _auth.SocialLoginAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh(RefreshTokenRequest request)
        {
            var result = await _auth.RefreshAsync(request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequest request)
        {
            await _auth.LogoutAsync(GetUserId(), request.RefreshToken);
            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            await _auth.ForgotPasswordAsync(request);
            return NoContent();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _auth.ResetPasswordAsync(request);
            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserProfileResponse>> GetProfile()
        {
            var result = await _auth.GetProfileAsync(GetUserId());
            return Ok(result);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult<UserProfileResponse>> UpdateProfile(UpdateProfileRequest request)
        {
            var result = await _auth.UpdateProfileAsync(GetUserId(), request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("me/avatar")]
        public async Task<ActionResult<UserProfileResponse>> UploadAvatar(IFormFile file)
        {
            var result = await _auth.UploadAvatarAsync(GetUserId(), file);
            return Ok(result);
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException());
        }
    }
}