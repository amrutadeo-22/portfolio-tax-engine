using Microsoft.AspNetCore.Builder;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Services;
using Portfolio.Api.Domain.Enums;
using Portfolio.Api.Domain.Repositories;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddScoped<FifoAccountingService>();
builder.Services.AddSingleton<PortfolioRepository>();
builder.Services.AddSingleton<TransactionRepository>(); 
builder.Services.AddSingleton<InMemoryTradeRepository>();
var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();  
app.MapGet("/", ()=>"Portfolio API running");
app.MapGet("/health",() => "OK");
 

app.Run();