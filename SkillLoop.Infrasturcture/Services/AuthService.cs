using AutoMapper;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkillLoop.Application.DTOs.Auth;
using SkillLoop.Application.Interfaces.IService;
using SkillLoop.Domain.Entities;
using SkillLoop.Infrasturcture.Data;
using SkillLoop.Infrasturcture.Security;

namespace SkillLoop.Infrasturcture.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokens;
        private readonly IOtpService _otp;
        private readonly IFileStorageService _files;
        private readonly ISocialLoginService _social;
        private readonly JwtOptions _jwt;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext db, ITokenService tokens, IOtpService otp, IFileStorageService files, ISocialLoginService social, IOptions<JwtOptions> jwt, IMapper mapper)
        {
            _db = db;
            _tokens = tokens;
            _otp = otp;
            _files = files;
            _social = social;
            _jwt = jwt.Value;
            _mapper = mapper;
        }

        public async Task<MessageResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            ValidatePassword(request.Password, request.ConfirmPassword);

            var email = request.Email.Trim().ToLowerInvariant();

            if (await _db.Users.AnyAsync(x => x.Email == email, cancellationToken))
                throw new InvalidOperationException("Email is already registered.");

            var user = CreateUser(request.FullName, email, null, BCrypt.Net.BCrypt.HashPassword(request.Password));
            user.EmailVerified = false;

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            await _otp.SendEmailVerificationAsync(user.Id, user.Email, cancellationToken);

            return new MessageResponse("Registration successful. Please check your email to verify your account.");
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users
                .Include(x => x.Wallet)
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.EmailVerified)
                throw new UnauthorizedAccessException("Please verify your email before logging in.");

            return await IssueTokensAsync(user, cancellationToken);
        }

        public async Task<AuthResponse> SocialLoginAsync(SocialLoginRequest request, CancellationToken cancellationToken = default)
        {
            var identity = await _social.ValidateAsync(request.Provider, request.IdToken, cancellationToken);

            var login = await _db.ExternalLogins
                .FirstOrDefaultAsync(x => x.Provider == identity.Provider && x.ProviderKey == identity.ProviderKey, cancellationToken);

            User? user = null;

            if (login != null)
            {
                user = await _db.Users
                    .Include(x => x.Wallet)
                    .FirstOrDefaultAsync(x => x.Id == login.UserId, cancellationToken);
            }

            if (user == null)
            {
                var email = identity.Email.Trim().ToLowerInvariant();
                user = await _db.Users
                    .Include(x => x.Wallet)
                    .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
            }

            if (user == null)
            {
                user = CreateUser(identity.FullName, identity.Email.Trim().ToLowerInvariant(), request.PhoneNumber?.Trim(), string.Empty);
                user.EmailVerified = true;

                _db.Users.Add(user);
                await _db.SaveChangesAsync(cancellationToken);
            }

            if (login == null)
            {
                _db.ExternalLogins.Add(new ExternalLogin
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Provider = identity.Provider,
                    ProviderKey = identity.ProviderKey
                });

                await _db.SaveChangesAsync(cancellationToken);
            }

            return await IssueTokensAsync(user, cancellationToken);
        }

        public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var tokenHash = _tokens.HashToken(request.RefreshToken);

            var token = await _db.RefreshTokens
                .Include(x => x.User)
                .ThenInclude(x => x.Wallet)
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow, cancellationToken);

            if (token == null)
                throw new UnauthorizedAccessException("Invalid refresh token.");

            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);

            return await IssueTokensAsync(token.User, cancellationToken);
        }

        public async Task LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var tokenHash = _tokens.HashToken(refreshToken);

            var token = await _db.RefreshTokens
                .FirstOrDefaultAsync(x => x.UserId == userId && x.TokenHash == tokenHash && x.RevokedAt == null, cancellationToken);

            if (token == null)
                return;

            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task VerifyEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (user.EmailVerified)
                throw new InvalidOperationException("Email is already verified.");

            await _otp.VerifyAsync(user.Id, request.Code, "EmailVerification", cancellationToken);

            user.EmailVerified = true;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task ResendEmailVerificationAsync(ResendEmailVerificationRequest request, CancellationToken cancellationToken = default)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            if (user.EmailVerified)
                throw new InvalidOperationException("Email is already verified.");

            await _otp.SendEmailVerificationAsync(user.Id, user.Email, cancellationToken);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var phone = request.PhoneNumber.Trim();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("No account is associated with this phone number.");

            await _otp.SendPasswordResetAsync(user.Id, phone, cancellationToken);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            ValidatePassword(request.NewPassword, request.ConfirmPassword);

            var phone = request.PhoneNumber.Trim();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("No account is associated with this phone number.");

            await _otp.VerifyAsync(user.Id, request.Code, "PasswordReset", cancellationToken);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _db.RefreshTokens
                .Where(x => x.UserId == user.Id && x.RevokedAt == null)
                .ExecuteUpdateAsync(x => x.SetProperty(t => t.RevokedAt, DateTime.UtcNow), cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await FindUserAsync(userId, cancellationToken);
            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var user = await FindUserAsync(userId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) && request.PhoneNumber != user.PhoneNumber)
            {
                var phone = request.PhoneNumber.Trim();
                var phoneExists = await _db.Users.AnyAsync(x => x.Id != userId && x.PhoneNumber == phone, cancellationToken);

                if (phoneExists)
                    throw new InvalidOperationException("Phone number is already registered.");

                user.PhoneNumber = phone;
            }

            _mapper.Map(request, user);
            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileResponse>(user);
        }

        public async Task<UserProfileResponse> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken = default)
        {
            var user = await FindUserAsync(userId, cancellationToken);

            user.AvatarUrl = await _files.UploadImageAsync(file, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileResponse>(user);
        }

        private User CreateUser(string fullName, string email, string? phone, string passwordHash)
        {
            var userId = Guid.NewGuid();

            return new User
            {
                Id = userId,
                FullName = fullName.Trim(),
                Email = email,
                PhoneNumber = phone,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                Wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Balance = 0,
                    RowVersion = Guid.NewGuid().ToByteArray()
                }
            };
        }

        private async Task<User> FindUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return user;
        }

        private async Task<AuthResponse> IssueTokensAsync(User user, CancellationToken cancellationToken)
        {
            var accessToken = _tokens.CreateAccessToken(user);
            var refreshToken = _tokens.CreateRefreshToken();

            _db.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = _tokens.HashToken(refreshToken),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays)
            });

            await _db.SaveChangesAsync(cancellationToken);

            return new AuthResponse(accessToken, refreshToken, _tokens.AccessTokenExpiresAt, _mapper.Map<UserProfileResponse>(user));
        }

        private static void ValidatePassword(string password, string confirmation)
        {
            if (password.Length < 8 || password != confirmation)
                throw new InvalidOperationException("Password must be at least 8 characters and match confirmation.");
        }
    }
}