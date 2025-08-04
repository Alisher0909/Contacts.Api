using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ContactsApi.Filters;

public class AsyncAutoValidation(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public static int OrderLowerThanModelStateInvalidFilter => -2001;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            var isParameterFromBodyOrQuery =
                parameter.BindingInfo?.BindingSource == BindingSource.Body ||
                parameter.BindingInfo?.BindingSource == BindingSource.Query;

            var canBeValidated = isParameterFromBodyOrQuery && parameter.ParameterType.IsClass;
            var parameterGenerictype = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

            if (canBeValidated && serviceProvider.GetService(parameterGenerictype) is IValidator validator)
            {
                var subject = context.ActionArguments[parameter.Name];
                var result = await validator.ValidateAsync(new ValidationContext<object?>(subject), context.HttpContext.RequestAborted);
                if (result.IsValid is false)
                    result.AddToModelState(context.ModelState, null);
            }
        }
        await next();
    }
}