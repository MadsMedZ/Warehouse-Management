var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5164")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Development setup: use HTTP locally.
app.UseCors("FlutterDev");

app.MapControllers();

app.Run();
