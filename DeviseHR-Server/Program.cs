using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// example for extraction
// string hrServerPort = configuration["HR_SERVER_PORT"];

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\" ",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("JhdjG78897&*hDWewfhjJHuii()()90*789^^&6%56rtRE#w3321@!#ER789*(UIJjhGHGFSXcVjkhaiuohRDIiu&(*iJJKUIO78(iu()*^&)))")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Check if the token is expired
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
    policy.RequireClaim("userType", "1"));

    options.AddPolicy("Manager", policy =>
    policy.RequireClaim("userType", "1", "2"));

    options.AddPolicy("Employee", policy =>
    policy.RequireClaim("userType", "1", "2", "3"));

    options.AddPolicy("EnableAddEmployees", policy =>
    policy.RequireClaim("enableAddEmployees", "true"));

    options.AddPolicy("EnableTerminateEmployees", policy =>
    policy.RequireClaim("enableTerminateEmployees", "true"));
});

// add the CORS middleware to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();

    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// must be above authorization
app.UseCors("AllowAnyOrigin");

// must be above authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();






app.Run();