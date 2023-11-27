using System;

namespace ChatGPTRunner.Data
{
    public class MyContext
    {
        public string APIKey { get; set; }
        public string DbPath { get; set;  }
        public bool DEBUGGING { get; set; }

        public MyContext()
        {
            bool debugging = false;
            if (bool.TryParse(Environment.GetEnvironmentVariable("DEBUGGING"), out debugging))
            {
                DEBUGGING = debugging;
            } else
            {
                DEBUGGING = false;
            }

            APIKey = Environment.GetEnvironmentVariable("openai:api_key");
            if (APIKey == null)
            {
                APIKey = "";
                Console.Error.WriteLine("Environment misconfigured. Need value for openai:api_key");
            }
            DbPath = Environment.GetEnvironmentVariable("ConnectionString:GenReqContext");
            if (DbPath == null)
            {
                DbPath = "";
                Console.Error.WriteLine("Environment misconfigured. Need value for ConnectionString:GenReqContext");
            }


        }
    }
}
