using System;

namespace ChatGPTRunner.Data
{
    public class MyContext
    {
        public string APIKey { get; set; }
        public string DbPath { get; }

        public MyContext()
        {
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
