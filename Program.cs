var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/healthcheck", () => "API is running!");
app.Run();
