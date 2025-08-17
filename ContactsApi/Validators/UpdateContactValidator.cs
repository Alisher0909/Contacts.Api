using ContactsApi.Abstractions;
using ContactsApi.Dtos;
using FluentValidation;

namespace ContactsApi.Validators;

public class UpdateContactValidator : AbstractValidator<UpdateContactDto>
{
    public UpdateContactValidator(IContactService service)
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.Id)
            .Cascade(CascadeMode.Stop)
            .Must(id => service.IsIdExistsAsync(id, CancellationToken.None)
                .GetAwaiter().GetResult())
            .WithMessage(id => $"Contact with id '{id}' doesn't exist.");

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
            .Must(email => !service.IsEmailExistsAsync(email, CancellationToken.None)
                .GetAwaiter().GetResult())
            .WithMessage("Email address must be unique.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(13)
            .Must(phone => phone is { Length: > 0 } &&
                        phone.StartsWith("+998") &&
                        phone[0] == '+' &&
                        phone.Skip(1).All(char.IsDigit))
            .WithMessage("'PhoneNumber' must start with '+998' and contain only digits after '+'.");
    }   
}