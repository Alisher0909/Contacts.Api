using FluentValidation;
using FluentValidation.AspNetCore;

namespace ContactsApi;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddFluentValidationAsyncAutoValidation(this IMvcBuilder builder)
    {
        return builder.AddMvcOptions(o =>
        {
            o.Filters.Add<AsyncAutoValidation>(AsyncAutoValidation.OrderLowerThanModelStateInvalidFilter);
        });
    }
}