using Microsoft.EntityFrameworkCore;
using DemoAPI.Data;
using DemoAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Use an in-memory database for quick dev
builder.Services.AddDbContext<ClientDb>(opt => opt.UseInMemoryDatabase("ClientDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c => { 
        c.SwaggerDoc("v1", new() { Title = "DemoAPI", Version = "v1" }); 
        c.EnableAnnotations();
    })
    .AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.SetIsOriginAllowed(origin => true);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });
    })
    .AddSignalR();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Seed database
using(var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<ClientDb>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => {
        o.SwaggerEndpoint("/swagger/v1/swagger.json","API v1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app
    .UseRouting()
    .UseEndpoints(endpoints => {
        endpoints.MapControllers();
        endpoints.MapHub<ClientHub>("/clientHub");
    });

app.Run();