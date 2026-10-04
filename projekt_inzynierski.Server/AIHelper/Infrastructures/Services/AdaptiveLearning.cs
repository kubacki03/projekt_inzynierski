using System.Text.Json;
using OpenAI.Chat;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Users.Domain.Models;

namespace projekt_inzynierski.Server.AIHelper.Infrastructures.Services
{
    public class AdaptiveLearning : IAdaptiveLearning
    {
        string apiKey = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY");
        public async Task<bool> CanBeCreated(string language, string knowledge)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);
            List<ChatMessage> messages = new()
            {
                new UserChatMessage($"Czy ma sens i czy można stworzyć kurs z {language} jesli chce sie nauczyc {knowledge}")
    };


            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
    jsonSchemaFormatName: "subject_to_learn",
    jsonSchema: BinaryData.FromBytes(""" 
    {
        "type": "object",
        "properties": {
            "response": { "type": "boolean" }
        },
        "required": ["response"],
        "additionalProperties": false
    }
    """u8.ToArray()),
    jsonSchemaIsStrict: true
)

            };


            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            var content = completion.Content[0].Text;

           
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            return json.GetProperty("response").GetBoolean();
        }
    }
}