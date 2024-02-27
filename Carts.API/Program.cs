using Carts.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions(builder);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddRedis(builder);
builder.Services.AddKafka(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();