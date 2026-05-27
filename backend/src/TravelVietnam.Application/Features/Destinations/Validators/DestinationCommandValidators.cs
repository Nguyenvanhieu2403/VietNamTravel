using FluentValidation;
using TravelVietnam.Application.Features.Destinations.Commands;

namespace TravelVietnam.Application.Features.Destinations.Validators;

public class CreateDestinationCommandValidator : AbstractValidator<CreateDestinationCommand>
{
    public CreateDestinationCommandValidator()
    {
        RuleFor(x => x.ProvinceId)
            .GreaterThan(0).WithMessage("Province ID must be valid");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Destination name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase with hyphens");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90d, 90d).When(x => x.Latitude.HasValue).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180d, 180d).When(x => x.Longitude.HasValue).WithMessage("Longitude must be between -180 and 180");

        RuleFor(x => x.EntryFee)
            .GreaterThanOrEqualTo(0).WithMessage("Entry fee must be zero or positive");

        RuleFor(x => x.ThumbnailUrl)
            .Must(x => x == null || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Thumbnail URL format is invalid");
    }
}

public class UpdateDestinationCommandValidator : AbstractValidator<UpdateDestinationCommand>
{
    public UpdateDestinationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Destination ID must be valid");

        RuleFor(x => x.ProvinceId)
            .GreaterThan(0).WithMessage("Province ID must be valid");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Destination name is required")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase with hyphens");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90d, 90d).When(x => x.Latitude.HasValue).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180d, 180d).When(x => x.Longitude.HasValue).WithMessage("Longitude must be between -180 and 180");

        RuleFor(x => x.EntryFee)
            .GreaterThanOrEqualTo(0).WithMessage("Entry fee must be zero or positive");

        RuleFor(x => x.ThumbnailUrl)
            .Must(x => x == null || Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Thumbnail URL format is invalid");
    }
}

public class DeleteDestinationCommandValidator : AbstractValidator<DeleteDestinationCommand>
{
    public DeleteDestinationCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Destination ID must be valid");
    }
}
