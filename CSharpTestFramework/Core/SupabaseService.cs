using Core.DbModels;
using Serilog;

namespace Core
{
    public class SupabaseService
    {
        private readonly DatabaseClient _dbClient;

        public SupabaseService(DatabaseClient dbClient)
        {
            _dbClient = dbClient;
        }

        public async Task<List<HomepageContent>> GetHomepageContentAsync()
        {
            try
            {
                var result = await _dbClient.Client.From<HomepageContent>().Get();
                return result.Models;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to retrieve homepage content from database.", ex);
                throw new InvalidOperationException("Database retrieval failed, see logs for more details.", ex);
            }
            
        }
    }
}
