using FluentValidation;
using TravelVietnam.Application.Features.Provinces.Commands;

namespace TravelVietnam.Application.Features.Provinces.Validators;

public class CreateProvinceCommandValidator : AbstractValidator<CreateProvinceCommand>
{
    public CreateProvinceCommandValidator()
    {
        RuleFor(x => x.RegionId)
            .GreaterThan(0).WithMessage("Region ID must be valid");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Province name is required")
            .MinimumLength(2).WithMessage("Province name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Province name cannot exceed 100 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase with hyphens only");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");

        RuleFor(x => x.CultureDescription)
            .MaximumLength(5000).WithMessage("Culture description cannot exceed 5000 characters");

        RuleFor(x => x.BestTimeToVisit)
            .MaximumLength(1000).WithMessage("Best time to visit cannot exceed 1000 characters");

        RuleFor(x => x.AverageBudget)
            .GreaterThan(0).WithMessage("Average budget must be greater than 0");

        RuleFor(x => x.VideoUrl)
            .Must(x => x == null || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Video URL format is invalid");

        RuleFor(x => x.ThumbnailUrl)
            .Must(x => x == null || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Thumbnail URL format is invalid");
    }
}
