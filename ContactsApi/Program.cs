using ContactsApi.Filters;
using ContactsApi.Models;
using ContactsApi.Validators;
using ContactsApi.Dtos;
using ContactsApi.Middlewares;
using ContactsApi.Services;
using FluentValidation;
using ContactsApi.Services.Abstractions;
using ContactsApi.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IValidator<CreateContactDto>, CreateContactValidator>();
builder.Services.AddScoped<IValidator<UpdateContactDto>, UpdateContactValidator>();
builder.Services.AddScoped<IValidator<PatchContactDto>, PatchContactValidator>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Contacts")));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();