using DeviceManagement.Config;
using DeviceManagement.service;
using DeviceManagement.settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;


var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

builder.Configuration["GROQ_API_KEY"] = Environment.GetEnvironmentVariable("GROQ_API_KEY") ?? throw new InvalidOperationException("GROQ_KEY environment variable is not set.");

builder.Services.AddDbContext<SystemDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<IGroqService, GroqService>();

builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Missing JwtSettings configuration.");

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "DeviceManagementAPI";
    config.Title = "DeviceManagement v1";
    config.Version = "v1";

    config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste only the JWT token value."
    });

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.RequireHttpsMetadata = false;
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };

    jwtOptions.MapInboundClaims = false;
});

builder.Services.AddAuthorization();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "DeviceManagement API Documentation";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}


app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
