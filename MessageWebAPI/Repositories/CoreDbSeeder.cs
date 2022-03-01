namespace MessageWebAPI.Repositories
{
    public class CoreDbSeeder
    {
        public CoreDbSeeder()
        {
            
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            //Based on EF team's example at https://github.com/aspnet/MusicStore/blob/dev/samples/MusicStore/Models/SampleData.cs
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var coreDb = serviceScope.ServiceProvider.GetService<CoreDbContext>();

                await coreDb.Database.EnsureCreatedAsync();
            }
        }
    }
}
