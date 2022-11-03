using Microsoft.AspNetCore;
using Services;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IServiceStatusCollector, ServicesStatusCollector>();
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();


