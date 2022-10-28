using Services;


var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IServiceStatusCollector, ServicesStatusCollector>();
builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();


