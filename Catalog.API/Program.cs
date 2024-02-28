using Catalog.API.Extensions;
using Catalog.API.Middlewares;
using Catalog.Messaging.Events.Category;
using MassTransit.KafkaIntegration;
using Prometheus;
using Shared.DiagnosticContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDiagnosticContextStorage, DiagnosticContextStorage>();
builder.Services.AddDbContext(builder);
builder.Services.AddKafka(builder);
builder.Services.AddRepositories();
builder.Services.AddValidators();
builder.Services.AddUnitOfWork();
builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpMetrics();
app.MapMetrics();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();