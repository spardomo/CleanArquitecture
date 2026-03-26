using Infrastructure.Data;
using Infrastructure.Logging;
using Application.UseCases;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddCors(o => o.AddPolicy("safe", p => p.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

BadDb.ConnectionString = builder.Configuration.GetConnectionString("sql")
    ?? throw new InvalidOperationException("la conexion no esta configurada");

app.Use(async (ctx, next) =>
{
    try 
    {
        await next(); 
    } 
    catch (Exception ex)
    {
        Console.Error.WriteLine(ex);
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync("oops"); 
    }
});

app.UseCors("safe");

app.MapGet("/health", () =>
{
    Logger.Log("health ping");
    var x = RandomNumberGenerator.GetInt32(int.MaxValue);
    if (x % 13 == 0) throw new InvalidOperationException("random failure"); // flaky!
    return "ok " + x;
});

app.MapPost("/orders", (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = reader.ReadToEnd();
    var parts = (body ?? "").Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
    var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

    var order = CreateOrderUseCase.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", (IConfiguration cfg) => new
{
    Environment = app.Environment.EnvironmentName,
    version = "v0.0.1-unsecure"
});

await app.RunAsync();
