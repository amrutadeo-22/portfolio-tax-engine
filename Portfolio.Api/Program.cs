using Microsoft.AspNetCore.Builder;
using Portfolio.Api.Domain.Entities;
using Portfolio.Api.Domain.Services;
using Portfolio.Api.Domain.Enums;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddScoped<FifoAccountingService>();

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();  
app.MapGet("/", ()=>"Portfolio API running");
app.MapGet("/health",() => "OK");
 

app.Run();