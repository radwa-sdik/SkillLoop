using Microsoft.EntityFrameworkCore;
using SkillLoop.Application.Interfaces.IService;
using SkillLoop.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SkillLoop.Infrasturcture.Services
{
    public class OtpService : IOtpService
    {
        private const string EmailVerification = "EmailVerification";
        private const string PasswordReset = "PasswordReset";

        private readonly Data.AppDbContext _db;
        private readonly IEmailService _email;
        private readonly ISmsService _sms;

        public OtpService(Data.AppDbContext db, IEmailService email, ISmsService sms)
        {
            _db = db;
            _email = email;
            _sms = sms;
        }

        public Task SendEmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken = default)
        {
            return SendAsync(userId, email, EmailVerification, cancellationToken);
        }

        public Task SendPasswordResetAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken = default)
        {
            return SendAsync(userId, phoneNumber, PasswordReset, cancellationToken);
        }

        public async Task VerifyAsync(Guid userId, string code, string purpose, CancellationToken cancellationToken = default)
        {
            if (purpose is not (EmailVerification or PasswordReset))
                throw new InvalidOperationException("Unsupported OTP purpose.");

            var otp = await _db.OtpCodes
                .Where(x => x.UserId == userId && x.Purpose == purpose && x.UsedAt == null)
                .OrderByDescending(x => x.ExpiresAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (otp == null || otp.ExpiresAt <= DateTime.UtcNow)
                throw new InvalidOperationException("OTP is invalid or expired.");

            if (otp.Attempts >= 5)
                throw new InvalidOperationException("Maximum OTP attempts exceeded.");

            otp.Attempts++;

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(code));
            var stored = Convert.FromBase64String(otp.CodeHash);

            if (!CryptographicOperations.FixedTimeEquals(hash, stored))
            {
                await _db.SaveChangesAsync(cancellationToken);
                throw new InvalidOperationException("OTP is invalid.");
            }

            otp.UsedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

        private async Task SendAsync(Guid userId, string destination, string purpose, CancellationToken cancellationToken)
        {
            var recentlySent = await _db.OtpCodes
                .AnyAsync(x => x.UserId == userId &&
                               x.Purpose == purpose &&
                               x.UsedAt == null &&
                               x.ExpiresAt > DateTime.UtcNow.AddMinutes(4), cancellationToken);

            if (recentlySent)
                throw new InvalidOperationException("Please wait before requesting another OTP.");

            var code = RandomNumberGenerator.GetInt32(1000, 10000).ToString();
            var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(code)));

            _db.OtpCodes.Add(new OtpCode
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CodeHash = hash,
                Purpose = purpose,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0
            });

            await _db.SaveChangesAsync(cancellationToken);

            if (purpose == EmailVerification)
            {
                await _email.SendAsync(
                    destination,
                    "SkillLoop Email Verification Code",
                    $"Your SkillLoop verification code is: {code}{Environment.NewLine}{Environment.NewLine}This code expires in 5 minutes.",
                    cancellationToken);
            }
            else
            {
                await _sms.SendAsync(
                    destination,
                    $"Your SkillLoop password reset code is: {code}. It expires in 5 minutes.",
                    cancellationToken);
            }
        }
    }
}