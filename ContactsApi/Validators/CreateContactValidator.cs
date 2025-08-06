using ContactsApi.Abstractions;
using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators;

public class CreateContactValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactValidator(IContactService service)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("'FirstName' must not be empty.")
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("'LastName' must not be empty.")
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email is { Length: > 0 })
            .WithMessage("Email address is invalid.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(13)
            .Must(phone => phone is { Length: > 0 } && 
                        phone.StartsWith("+998") && 
                        phone[0] == '+' && 
                        phone.Skip(1).All(char.IsDigit))
            .WithMessage("PhoneNumber must start with '+998' and contain only digits after '+'.");
    }   
}