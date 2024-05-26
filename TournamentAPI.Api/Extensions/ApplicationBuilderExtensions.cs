
namespace TournamentAPI.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            // WHAT DOES THIS DO?
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TournamentAPIContext>();

                try
                {
                    await SeedData.Initialize(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not seed database");
                    throw;
                }
            }
        }
    }
}
