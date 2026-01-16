using SmartService.API.GraphQL;
using SmartService.Application;
using SmartService.Application.Abstractions.AI;
using SmartService.Application.UseCases.AnalyzeServiceRequest;
using SmartService.Infrastructure;
using SmartService.Infrastructure.AI.Ollama;
using SmartService.Infrastructure.KnowledgeBase.Complexity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddGraphQLServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<OllamaClient>();
builder.Services.AddScoped<AnalyzeServiceRequestHandler>();


builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.MapControllers(); 
app.MapGraphQL();

app.Run();

