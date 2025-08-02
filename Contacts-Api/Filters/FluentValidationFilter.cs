using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsApi.Filters;

public class FluentValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments)
        {
            if (argument.Value is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.Value.GetType());
            var validator = _serviceProvider.GetService(validatorType) as IValidator;

            if (validator is not null)
            {
                var validationContext = new ValidationContext<object>(argument.Value!);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    var problemDetails = new ValidationProblemDetails(errors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "One or more validation errors occurred."
                    };

                    context.Result = new BadRequestObjectResult(problemDetails);
                    return;
                }
            }
        }

        await next();
    }
}