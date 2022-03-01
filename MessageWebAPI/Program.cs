using MessageWebAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);

            options.TokenValidationParameters.NameClaimType = "name";
        },
options => { builder.Configuration.Bind("AzureAdB2C", options); });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkNpgsql()
                .AddDbContext<CoreDbContext>(options =>
                options.UseNpgsql(builder.Configuration["Data:DbContext:CoreConnectionString"]));

builder.Services.AddScoped<ICoreRepository, CoreRepository>();
builder.Services.AddTransient<CoreDbSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var coreDb = serviceScope.ServiceProvider.GetService<CoreDbContext>();

    await coreDb.Database.EnsureCreatedAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
