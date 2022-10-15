using Microsoft.EntityFrameworkCore;
using NotificationSchedulingSystem.Business.Services;
using NotificationSchedulingSystem.Infrastructure;
using NotificationSchedulingSystem.Infrastructure.Repos;
using NotificationSchedulingSystem.API.Middlewares;
using System.Text.Json.Serialization;
using NLog.Extensions.Logging;
using FluentValidation.AspNetCore;
using FluentValidation;
using NotificationSchedulingSystem.Business.Models.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NotificationSchedulingSystemContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddSingleton<ICreateNotificationsService, CreateNotificationsService>();

builder.Services.AddValidatorsFromAssemblyContaining<CompanyRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddLogging(loggingBuilder =>
{
    // configure Logging with NLog
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.AddNLog();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
