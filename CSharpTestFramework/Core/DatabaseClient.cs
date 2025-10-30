using Supabase;
using Supabase.Interfaces;
using Supabase.Postgrest;

namespace Core
{
    public class DatabaseClient
    {
        private Supabase.Client _client;

        public async Task InitializeAsync()
        {
            DotNetEnv.Env.Load();
            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

            var options = new Supabase.SupabaseOptions { AutoConnectRealtime = true };
            var _client = new Supabase.Client(url, key, options);

            await _client.InitializeAsync();
        }

        public Supabase.Client Client => _client ?? throw new InvalidOperationException("Database client not initialized.");

        // A result can be fetched like so.
        public async Task<List<T>> GetAllAsync<T>() where T : Supabase.Postgrest.Models.BaseModel, new()
        {
            var result = await _client.From<T>().Get();
            return result.Models;
        }

    }



}
