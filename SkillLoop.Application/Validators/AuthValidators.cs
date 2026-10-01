using FluentValidation;
using SkillLoop.Application.DTOs.Auth;

namespace SkillLoop.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
        }
    }

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }

    public class SocialLoginRequestValidator : AbstractValidator<SocialLoginRequest>
    {
        public SocialLoginRequestValidator()
        {
            RuleFor(x => x.Provider)
                .NotEmpty()
                .Must(x => x.Equals("google", StringComparison.OrdinalIgnoreCase) ||
                           x.Equals("apple", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Provider must be Google or Apple.");

            RuleFor(x => x.IdToken)
                .NotEmpty();

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Phone number must be a valid international phone number.");
        }
    }

    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }

    public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Code)
                .NotEmpty()
                .Matches(@"^\d{4}$")
                .WithMessage("Verification code must be exactly 4 digits.");
        }
    }

    public class ResendEmailVerificationRequestValidator : AbstractValidator<ResendEmailVerificationRequest>
    {
        public ResendEmailVerificationRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }

    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .WithMessage("Phone number must be a valid international phone number.");
        }
    }

    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .WithMessage("Phone number must be a valid international phone number.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .Matches(@"^\d{4}$")
                .WithMessage("Reset code must be exactly 4 digits.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Passwords do not match.");
        }
    }

    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Headline)
                .MaximumLength(200)
                .When(x => x.Headline != null);

            RuleFor(x => x.City)
                .MaximumLength(100)
                .When(x => x.City != null);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{6,14}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Phone number must be a valid international phone number.");
        }
    }
}