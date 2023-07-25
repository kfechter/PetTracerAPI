﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetTracerAPI.Models;
using PetTracerAPI.Services;

var firebaseAPIKey = Environment.GetEnvironmentVariable("FirebaseKey");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add MongoDB Support
builder.Services.Configure<PetTracerDatabaseSettings>(
    builder.Configuration.GetSection("PetTracerDatabase"));

builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<PetsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the firebase JWTBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseAPIKey}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseAPIKey}",
            ValidateAudience = true,
            ValidAudience = firebaseAPIKey,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

