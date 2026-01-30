using Microsoft.EntityFrameworkCore;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Data;
using UserService.Application.Services;
using UserService.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserServiceImp>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/users", async (string firstName, string lastName, string email, string passwordHash, string role, IUserService service) =>
{
    await service.AddUserAsync(new User(firstName, lastName, email, passwordHash, role)
    );
    return Results.Ok();
});

app.MapGet("/users/{id}", async (int id, IUserService service) =>
{
    var user = await service.GetUserByIdAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

app.Run();