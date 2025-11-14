using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TestConfig
    {
        public static string BaseUrl
        {
            get
            {
                var env = Environment.GetEnvironmentVariable("TARGET_ENVIRONMENT")?.ToLower();

                return env switch
                {
                    "local" => "http://127.0.0.1:5000",
                    "prod" => "https://dohertyalex.cc",
                    _ => "https://dohertyalex.cc"
                };
            }
        }
    }
}
