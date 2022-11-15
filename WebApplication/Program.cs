using Microsoft.AspNetCore;
using Microsoft.Extensions.Options;
using Services;
using ServiseEntities;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IServiceStatusCollector, ServicesStatusCollector>();
builder.Services.AddControllers();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});
builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy.WithOrigins(
                    "https://localhost:44398",
                    "http://localhost:4200",
                    "http://localhost:80")
                .AllowAnyMethod();
        });
});
var app = builder.Build();

app.MapControllers();
app.UseCors("MyPolicy");
app.Run();

