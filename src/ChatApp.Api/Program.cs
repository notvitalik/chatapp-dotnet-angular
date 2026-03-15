using ChatApp.Api;
using ChatApp.Api.Extensions;
using ChatApp.Application.DependencyInjection;
using ChatApp.Infrastructure.DependencyInjection;
using ChatApp.Infrastructure.Realtime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(ApiConstants.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHub<ChatHub>("/hubs/chat");
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program;
