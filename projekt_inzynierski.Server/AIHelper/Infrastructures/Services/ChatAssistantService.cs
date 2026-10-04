using OpenAI.Chat;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;

namespace projekt_inzynierski.Server.AIHelper.Infrastructures.Services
{
    public class ChatAssistantService : IChatAssistant
    {
        public async Task<string> ChatResponse(string message)
        {
            string OPENAI_API_KEY = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY");
            ChatClient client = new(model: "gpt-4o", apiKey: OPENAI_API_KEY);

            ChatCompletion completion = await client.CompleteChatAsync("Jestes asystentem AI w platformie do nauki programowania, odpowiadaj jedynie na pytania w temacie informatyki. Oto pytanie:"+message);
            
            return completion.Content[0].Text;
        }
    }
}