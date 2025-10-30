using Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = await _dbClient.Client.From<HomepageContent>().Get();
            return result.Models;
        }
    }
}
