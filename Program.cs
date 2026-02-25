var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<frameworks_pr1.Middleware.RequestIdMiddleware>();
app.UseMiddleware<frameworks_pr1.Middleware.ErrorHandlingMiddleware>();
app.UseMiddleware<frameworks_pr1.Middleware.LoggingMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();