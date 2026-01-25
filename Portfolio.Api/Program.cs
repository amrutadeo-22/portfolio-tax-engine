using Microsoft.AspNetCore.Builder;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Services;
using Portfolio.Api.Domain.Enums;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FifoAccountingService>();
var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", ()=>"Portfolio API running");
app.MapGet("/health",() => "OK");
app.MapPost("/api/v1/fifo/calculate", (IEnumerable<Transaction> transactions, FifoAccountingService service) =>
{
    var result = service.Calculate(transactions);
    return Results.Ok(result);
});

app.Run();