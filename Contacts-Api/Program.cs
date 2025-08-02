using ContactsApi.Filters;
using ContactsApi.Middlewares;
using ContactsApi.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();