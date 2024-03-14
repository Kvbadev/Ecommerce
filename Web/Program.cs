using System.Text;
using Core;
using Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Photos;
using Infrastructure.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["AllowedOrigin"]);
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
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtOpt => 
    {
        jwtOpt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"])),
        };
});



//Database
builder.Services.AddDbContext<DataContext>(options => 
{
    // options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTION"));
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();


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

app.UseDefaultFiles(); //wwwroot files
app.UseStaticFiles(); //serve static files from wwwroot

// Security headers
app.UseXContentTypeOptions();
app.UseReferrerPolicy(o => o.NoReferrerWhenDowngrade());
app.UseXXssProtection(o => o.EnabledWithBlockMode());
app.UseXfo(o => o.Deny());
// app.UseCsp(o => o.UpgradeInsecureRequests()
//     .StyleSources(s => s.Self().UnsafeInline().CustomSources("https://fonts.googleapis.com/", "https://fonts.gstatic.com/", "https://accounts.google.com/gsi/"))
//     .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com/"))
//     .FormActions(s => s.Self())
//     .FrameAncestors(s => s.Self())
//     .ImageSources(s => s.Self().CustomSources("http://res.cloudinary.com/", "https://dub.stats.paypal.com/", "https://c.sandbox.paypal.com/v1/", "https://b.stats.paypal.com/"))
//     .ScriptSources(s => s.Self().UnsafeInline().CustomSources("https://accounts.google.com/gsi/", "https://c.paypal.com"))
// );

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
    var helper = new CloudinaryHelper(builder.Configuration );
    var photos = await helper.GetAllPhotosUrls();
    await Seed.SeedData(scope.ServiceProvider, photos);
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToController("Index", "Frontend");

app.Run();
