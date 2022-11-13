using System.Text;
using System.Text.Json;
using Core;
using Data;
using Infrastructure;
using Infrastructure.Photos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Services;

//TODO: security things

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173", "http://localhost");
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddLogging();


//Identity: password
builder.Services.Configure<IdentityOptions>(opt => 
{
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequiredLength = 6;
});

//Authentiaction & Authorization
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
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"]))
        };
});



builder.Services.AddDbContext<DataContext>(options => 
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});



builder.Services.AddIdentityCore<AppUser>(opt => 
{
    opt.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager<SignInManager<AppUser>>();


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//has to be scoped because of usage of userManager to obtain user's roles
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
var app = builder.Build();

app.UseRouting();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}
else
{
    //Exception Middleware
    app.UseExceptionHandler(exHandler => 
    {
        exHandler.Run(async context => 
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync($"Server Error: {context.Response.StatusCode}");
        });
    });
}

using (var scope = app.Services.CreateScope())
{
    var helper = new CloudinaryHelper(builder.Configuration);
    var photos = await helper.GetAllPhotosUrls();
    await Seed.SeedData(scope.ServiceProvider, photos);
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
