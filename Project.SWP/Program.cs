using Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Project.SWP.Middlewares;
using Project.SWP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.BuildServices(builder.Configuration);
builder.Services.CoreServices();

// add cors
builder.Services.AddCors();
// add authen policy
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => builder.Configuration.Bind("JWTSection", options));

builder.Services.AddAuthorization(options =>
{

    options.AddPolicy(IdentityData.Staff, policy => policy.RequireRole("Staff", "Admin", "System"));
    options.AddPolicy(IdentityData.Admin, policy => policy.RequireRole("Admin", "System"));
    options.AddPolicy(IdentityData.Customer, policy => policy.RequireRole("Customer", "Admin", "System"));
    options.AddPolicy(IdentityData.Guest, policy => policy.RequireRole("Guest", "System"));
    options.AddPolicy(IdentityData.Intructors, policy => policy.RequireRole("Intructors", "System"));
    options.AddPolicy(IdentityData.System, policy => policy.RequireRole("System"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Middleware
app.UseHttpsRedirection();

app.UseCors();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
