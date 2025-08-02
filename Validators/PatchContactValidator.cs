using ContactsApi.Dtos;
using ContactsApi.Services;
using FluentValidation;

namespace ContactsApi.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>
{
    public PatchContactValidator(IContactService service, IHttpContextAccessor httpContextAccessor)
    {
        var routeId = () =>
        {
            var idStr = httpContextAccessor.HttpContext?.Request.RouteValues["id"]?.ToString();
            return int.TryParse(idStr, out var id) ? id : int.MaxValue;
        };

        RuleFor(x => x.FirstName)
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$")
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .MinimumLength(2).MaximumLength(50)
            .Matches(@"^[\p{L}\s\-]+$")
            .When(x => x.LastName is not null);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MustAsync(async (email, ct) =>
            {
                var id = routeId();
                return !await service.EmailExistsAsync(email!, id);
            }).WithMessage("Email is already in use.")
            .When(x => x.Email is not null);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+998\d{9}$")
            .MustAsync(async (phone, ct) =>
            {
                var id = routeId();
                return !await service.PhoneExistsAsync(phone!, id);
            }).WithMessage("Phone number is already in use.")
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Address)
            .MinimumLength(5).MaximumLength(200)
            .When(x => x.Address is not null);
    }
}