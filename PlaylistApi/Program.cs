using Microsoft.EntityFrameworkCore;
using PlaylistApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add db context to the services container
builder.Services.AddDbContext<PlaylistDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlaylistDbConnection"));
});

// Adding repository implementation for IPlaylistRepository dependency request
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Populate database data
SeedData.EnsurePopulated(app);

app.Run();
