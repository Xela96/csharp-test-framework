#nullable disable warnings
using Serilog;

namespace Core
{
    public class DatabaseClient(Supabase.Client _client)
    {        

        public async Task InitializeAsync()
        {
            try
            {
                DotNetEnv.Env.Load();
            }
            catch (FileNotFoundException)
            {
                Log.Warning(".env file not found in current directory");
            }

            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

            try
            {
                var options = new Supabase.SupabaseOptions { AutoConnectRealtime = true };
                _client = new Supabase.Client(url, key, options);

                await _client.InitializeAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Failed to initialise connection with Supabase database.", ex);
                throw new InvalidOperationException("Database connection failed, see logs for more details.", ex);
            }
            
        }

        public Supabase.Client Client => _client ?? throw new InvalidOperationException("Database client not initialized.");

        public async Task<List<T>> GetAllAsync<T>() where T : Supabase.Postgrest.Models.BaseModel, new()
        {
            try
            {
                var result = await _client.From<T>().Get();
                return result.Models;
            }
            catch(Exception ex) 
            {
                Log.Error("Failed to retrieve content from database table.", ex);
                throw new InvalidOperationException("Database retrieval failed, see logs for more details.", ex);
            }
            
        }

    }



}

#nullable enable warnings