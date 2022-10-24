using Microsoft.EntityFrameworkCore;
using DemoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Use an in-memory database for quick dev
builder.Services.AddDbContext<ClientDb>(opt => opt.UseInMemoryDatabase("ClientDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Open the API to everyone - in practice, you'd want to whitelist specific domains - ST
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});

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

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Simple API Code
// In a larger project, this should be separated into Controllers, Business Logic, and Interfaces
// - Sean Talley
/////////////////////////////////////////////////////////////////////////////////////////////////////
app.MapGet("/ClientInfo", async (ClientDb db) =>
    await db.ClientInfos.ToListAsync());
app.MapGet("/ClientInfo/{id}", async (int id, ClientDb db) =>
    await db.ClientInfos.FindAsync(id)
        is ClientInfo clientInfo
            ? Results.Ok(clientInfo)
            : Results.NotFound());
app.MapPost("/ClientInfo", async (ClientInfo clientInfo, ClientDb db) =>
{
    db.ClientInfos.Add(clientInfo);
    await db.SaveChangesAsync();
    return Results.Created($"/ClientInfo/{clientInfo.Id}", clientInfo);
});
app.MapPut("/ClientInfo/{id}", async (int id, ClientInfo clientInfo, ClientDb db) =>
{
    var currentInfo = await db.ClientInfos.FindAsync(id);
    if (currentInfo is null) return Results.NotFound();
    currentInfo.FirstName = clientInfo.FirstName;
    currentInfo.LastName = clientInfo.LastName;
    currentInfo.Email = clientInfo.Email;
    currentInfo.LastUpdated = DateTime.Now;
    await db.SaveChangesAsync();
    return Results.Ok(currentInfo);
});
app.MapDelete("/ClientInfo/{id}", async (int id, ClientDb db) =>
{
    if (await db.ClientInfos.FindAsync(id) is ClientInfo clientInfo)
    {
        db.ClientInfos.Remove(clientInfo);
        await db.SaveChangesAsync();
        return Results.Ok(clientInfo);
    }
    return Results.NotFound();
});
//////////////////////////////////////////////////////////////////////////////

app.Run();