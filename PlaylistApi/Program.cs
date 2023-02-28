using Microsoft.EntityFrameworkCore;
using PlaylistApi.Data;
using PlaylistApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register automapper as a service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add db context to the services container
builder.Services.AddDbContext<PlaylistDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlaylistDbConnection"));
});

// Adding repository implementation for IPlaylistRepository dependency request
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlaylistService>();

// Populate database data
SeedData.EnsurePopulated(app);

app.Run();
