using DockerInfnetDevOpsSample.HealthCheck;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddHealthChecks()
                .AddSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("InfnetPosDb"),
                    healthQuery: "SELECT 1",
                    name: "Database",
                    failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
                ).AddUrlGroup(new Uri("http://httpbin.org/status/200"), "Api Terceiro Nao Autenticada")
                .AddCheck<HealthCheckRandom>(name: "Api Terceiro Autenticada"); ;
               

builder.Services.AddHealthChecksUI(s =>
{
    s.AddHealthCheckEndpoint("Infnet API", "https://localhost:44364/healthz");
}).AddInMemoryStorage();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting()
   .UseEndpoints(config =>
   {
       config.MapHealthChecks("/healthz", new HealthCheckOptions
       {
           Predicate = _ => true,
           ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
       });

       config.MapHealthChecksUI();
   });

app.Run();
