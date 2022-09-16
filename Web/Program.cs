using System.Text;
using AutoMapper;
using Core;
using Data;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Web.Services;
using Web.Services.JwtToken;


//TODO: if foreign key contraints failed - CHECK IF ID HASN'T CHANGED  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173");
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddLogging();



builder.Services.Configure<IdentityOptions>(opt => 
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequiredLength = 6;
});



builder.Services.AddAuthentication(opt => 
{
    // opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtOpt => 
    {
        jwtOpt.Audience = "https://localhost:5000";
        jwtOpt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"]))
        };
});

builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddIdentityCore<AppUser>(opt => 
{
    opt.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

var app = builder.Build();

app.UseRouting();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await Seed.SeedData(context);
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();
