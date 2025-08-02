using ContactsApi.Dtos;
using ContactsApi.Services;
using FluentValidation;

namespace ContactsApi.Validators;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator(IContactService service)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("First name may only contain letters, hyphens, and spaces.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Last name may only contain letters, hyphens, and spaces.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress()
            .MustAsync(async (email, ct) =>
                !await service.EmailExistsAsync(email))
            .WithMessage("Email is already in use.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+998\d{9}$").WithMessage("Phone number must be in +998XXXXXXXXX format.")
            .MustAsync(async (phone, ct) =>
                !await service.PhoneExistsAsync(phone))
            .WithMessage("Phone number is already in use.");

        RuleFor(x => x.Address)
            .MinimumLength(5).MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}