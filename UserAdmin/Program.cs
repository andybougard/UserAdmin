using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;
using UserAdmin.Data;
using UserAdmin.Data.Entities;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Graylog(
        new GraylogSinkOptions
        {
            HostnameOrAddress = "localhost",
            Port = 12201,
            Facility = "UserAdmin",
            TransportType = TransportType.Udp
        })
        .CreateLogger();

Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "UserAdmin API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>() }
    });
});

// Configure Entity Framework and Identity with SQL Server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainConnection")));

builder.Services.AddSingleton(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoConnection");
    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("MongoDB connection string is not configured.");
    return new MongoDBContext(connectionString, "sample_mflix");
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication( auth =>
{
    auth.DefaultAuthenticateScheme = auth.DefaultChallengeScheme = auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer( jwt =>
{
    jwt.SaveToken = false;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Appsettings:JWTSecret"]!)),
        ValidateAudience = true,
        ValidAudience = builder.Configuration["AppSettings:JWTAudience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["AppSettings:JWTIssuer"]
    };
});
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // User settings
    options.User.RequireUniqueEmail = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Authorization"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserAdmin API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
