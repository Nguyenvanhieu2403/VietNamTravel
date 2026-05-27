using FluentValidation;

namespace TravelVietnam.Application.Validators;

public class SlugValidator : AbstractValidator<string>
{
    public SlugValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Slug cannot be empty")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase with hyphens only, no spaces");
    }
}

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
            .Matches(@"[!@#$%^&*]").WithMessage("Password must contain at least one special character (!@#$%^&*)");
    }
}

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");
    }
}

public class NameValidator : AbstractValidator<string>
{
    public NameValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
    }
}

public class UrlValidator : AbstractValidator<string>
{
    public UrlValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("URL is required")
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("URL format is invalid");
    }
}

public class BudgetValidator : AbstractValidator<decimal>
{
    public BudgetValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0).WithMessage("Budget must be greater than 0");
    }
}

public class CoordinateValidator : AbstractValidator<double>
{
    public CoordinateValidator()
    {
        RuleFor(x => x)
            .InclusiveBetween(-180, 180).WithMessage("Latitude/Longitude must be between -180 and 180");
    }
}
