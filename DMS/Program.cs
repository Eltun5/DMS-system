using Serilog;
using WebApplication1.Extensions;
using WebApplication1.SignalR;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/AppLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration.AddJsonFile("logMessages.json", optional: false, reloadOnChange: true);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DatabaseInitializer.Initialize(services);
}

app.UseApplicationPipeline(app.Environment);

app.MapHub<ChatHub>("chat-hub");
app.MapControllers();

app.Run();