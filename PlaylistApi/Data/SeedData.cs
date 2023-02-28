using Microsoft.EntityFrameworkCore;
using Playlist;

namespace PlaylistApi.Data
{
    /// <summary>
    /// Represents the class responsible for populating the database.
    /// </summary>
    public class SeedData
    {
        /// <summary>
        /// Ensures that database contains initial data.
        /// </summary>
        /// <param name="app">Application builder that used to get <see cref="DbContext" />.</param>
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                PlaylistDbContext context = scope.ServiceProvider.GetRequiredService<PlaylistDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Songs.Any())
                {
                    PopulateData(context);
                }
            }
        }

        /// <summary>
        /// Adds default entries to the database.
        /// </summary>
        /// <param name="context"><see cref="PlaylistDbContext" /> exemplar that used to populate the database.</param>
        private static void PopulateData(PlaylistDbContext context)
        {
            context.Songs.AddRange(
                new Song() { Name = "Gila", Author = "Beach House", Duration = 286 },
                new Song() { Name = "Call Across Rooms", Author = "Grouper", Duration = 179 },
                new Song() { Name = "The Beach", Author = "The Neighbourhood", Duration = 255 },
                new Song() { Name = "Darling", Author = "Dreams We've Had", Duration = 301 },
                new Song() { Name = "Vanished", Author = "Crystal Castles", Duration = 243 }
            );

            context.SaveChanges();
        }
    }
}