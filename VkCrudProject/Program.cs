using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Repositories;
using VkCrudProject.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddScoped<IUserStateRepository, UserStateRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UserDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
