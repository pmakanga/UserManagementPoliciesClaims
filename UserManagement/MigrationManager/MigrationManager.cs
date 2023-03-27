using Microsoft.EntityFrameworkCore;
using UserManagement.Data;

namespace UserManagement.MigrationManager
{
    public static class MigrationManager
    {
        public static IHost MigrationDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

            return host;
        }
    }
}
