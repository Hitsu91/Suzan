using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Suzan.Application.Data;
using Suzan.Application.Services.AuthService;
using Suzan.Application.Services.CategoryService;
using Suzan.Application.Services.IdentityService;
using Suzan.Application.Services.RecipeService;
using Suzan.Application.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// START Configs //
const string dbPath = "Data Source=recipes.db";
builder.Services.AddSqlite<DataContext>(dbPath);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["AppSettings:Token"];
        var key = System.Text.Encoding.UTF8.GetBytes(secret);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
// END Configs //

// START Services //
var services = builder.Services;
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<IIdentityService, IdentityService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<IRecipeService, RecipeService>();
services.AddScoped<IUserService, UserService>();
// END Services //

// START CORS //
const string allowedOriginPolicy = "forDevelopment";
services.AddCors(options =>
{
    options.AddPolicy(allowedOriginPolicy, corsPolicyBuilder =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:4200");
    });
});
// END CORS //

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(allowedOriginPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
