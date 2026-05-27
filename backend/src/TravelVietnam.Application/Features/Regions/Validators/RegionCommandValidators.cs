using FluentValidation;
using TravelVietnam.Application.Features.Regions.Commands;

namespace TravelVietnam.Application.Features.Regions.Validators;

public class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
{
    public CreateRegionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Region name is required")
            .MinimumLength(2).WithMessage("Region name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Region name cannot exceed 100 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase with hyphens only");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");
    }
}

public class UpdateRegionCommandValidator : AbstractValidator<UpdateRegionCommand>
{
    public UpdateRegionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Region ID must be valid");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Region name is required")
            .MinimumLength(2).WithMessage("Region name must be at least 2 characters")
            .MaximumLength(100).WithMessage("Region name cannot exceed 100 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug must be lowercase with hyphens only");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Description cannot exceed 5000 characters");
    }
}

public class DeleteRegionCommandValidator : AbstractValidator<DeleteRegionCommand>
{
    public DeleteRegionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Region ID must be valid");
    }
}
