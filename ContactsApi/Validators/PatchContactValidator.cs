using ContactsApi.Abstractions;
using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>
{
    public PatchContactValidator(IContactService service)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(c => c.Id)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, token) => await service.IsIdExistsAsync(id, token))
            .WithMessage((dto, id) => $"Contact with id '{id}' doesn't exist.");
        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+998\d{9}$")
            .MustAsync(async (phoneNumber, token) => await service.IsPhoneExistsAsync(phoneNumber, token) is false)
            .WithMessage("Contact phone number must be unique");
    }
}