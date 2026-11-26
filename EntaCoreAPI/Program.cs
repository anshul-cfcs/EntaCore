using EntaCoreAPI.Extensions;
using EntaCoreAPI.Middlewares;
using Infrastructure.Services.Database;

var builder = WebApplication.CreateBuilder(args);

DapperTypeHandlers.Register();
builder.Services.AddProjectDependencies(builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseGlobalExceptionMiddleware();

app.UseHttpsRedirection();
app.UseAuthentication();
//app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
