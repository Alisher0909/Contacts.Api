using ContactsApi.Abstractions;
using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators
{
    public class PatchContactValidator : AbstractValidator<PatchContactDto>
    {
        public PatchContactValidator(IContactService service)
        {
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
            
            RuleFor(c => c.Id)
                .Cascade(CascadeMode.Stop)
                .Must(id => service.IsIdExistsAsync(id, default).GetAwaiter().GetResult())
                .WithMessage((dto, id) => $"Contact with id '{id}' doesn't exist.");

            RuleFor(c => c.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^\+998\d{9}$")
                .Must(phoneNumber => service.IsPhoneExistsAsync(phoneNumber, default).GetAwaiter().GetResult() is false)
                .WithMessage("Contact phone number must be unique");
        }
    }
}