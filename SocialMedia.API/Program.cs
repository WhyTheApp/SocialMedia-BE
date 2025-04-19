using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.API.Requests;
using SocialMedia.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<AddRegisterRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddDbContext<SocialMediaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



var allowedOrigins = new[] {
    "http://localhost:3000",
    "http://localhost:3001",
    "http://localhost:3002"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root URL
});

app.UseAuthorization();
app.MapControllers();

app.Run();