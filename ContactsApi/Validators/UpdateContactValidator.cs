using ContactsApi.Dtos;
using ContactsApi.Services;
using FluentValidation;

namespace ContactsApi.Validators;

public class UpdateContactValidator : AbstractValidator<UpdateContactDto>
{
    public UpdateContactValidator(IContactService service, IHttpContextAccessor httpContextAccessor)
    {
        var routeId = () =>
        {
            var idStr = httpContextAccessor.HttpContext?.Request.RouteValues["id"]?.ToString();
            return int.TryParse(idStr, out var id) ? id : int.MaxValue;
        };

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, ct) =>
            {
                var id = routeId();
                return !await service.EmailExistsAsync(email, id);
            }).WithMessage("Email is already in use.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+998\d{9}$")
            .MustAsync(async (phone, ct) =>
            {
                var id = routeId();
                return !await service.PhoneExistsAsync(phone, id);
            }).WithMessage("Phone number is already in use.");

        RuleFor(x => x.Address)
            .MinimumLength(5).MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}