using ChatGPTRunner.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;
using ChatGPTRunner.Data;

namespace ChatGPTRunner.Services
{
    public class ChatGPT
    {
        public MyContext MyAppContext { get; set; }
        public ChatGPT(MyContext cntxt)
        {
            MyAppContext = cntxt;
        }

        /**
         * reference https://platform.openai.com/docs/api-reference/making-requests
         */
        public async Task<List<Choice>> GetContentAsync(CompletionRequest req)
        {
            Uri baseAddress = new Uri("https://api.openai.com/v1/chat/completions");
            HttpClient _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", MyAppContext.APIKey);
            string jsonStr = JsonSerializer.Serialize(req);
            var xjsonContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            HttpResponseMessage resp = await _client.PostAsync(baseAddress, xjsonContent);

            List<Choice> responseToCaller = null;
            if (resp.IsSuccessStatusCode)
            {
                string data = resp.Content.ReadAsStringAsync().Result;
                CompletionResponse rspns = JsonSerializer.Deserialize<CompletionResponse>(data);
                responseToCaller = rspns.choices;
//                cntnt = rspns.choices[0].message.content;
//                Console.WriteLine("response is" + cntnt);
            }
            else
            {
                Console.WriteLine("Status code: " + resp.StatusCode);
                string str = await resp.Content.ReadAsStringAsync();
                Console.WriteLine("Response content: " + str);
            }
            return responseToCaller;
        }
    }
}
