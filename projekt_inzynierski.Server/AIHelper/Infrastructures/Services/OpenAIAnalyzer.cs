using OpenAI.Chat;
using System.Text.Json;
using projekt_inzynierski.Server.AIHelper.Application.Interfaces;
using projekt_inzynierski.Server.Content.Application.DTOs;
using projekt_inzynierski.Server.CodeRunner.Api;
using System.Threading.Tasks;

namespace projekt_inzynierski.Server.AIHelper.Infrastructures.Services
{
    public class OpenAIAnalyzer : ICodeAnalyzer
    {

        string apiKey = Environment.GetEnvironmentVariable("OPEN_AI_API_KEY");

        public async Task<CodeReviewResult> AnalyzeCode(string code, string task)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

            List<ChatMessage> messages = new()
    {
        new UserChatMessage($"Czy ten kod {code} jest poprawnym rozwiązaniem tego zadania {task}?")
    };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "code_review",
                    jsonSchema: BinaryData.FromBytes("""
            {
                "type": "object",
                "properties": {
                    "code_review": {
                        "type": "array",
                        "items": {
                            "type": "object",
                            "properties": {
                                "isDoneGood": { "type": "boolean" },
                                "review": { "type": "string" }
                            },
                            "required": ["isDoneGood", "review"],
                            "additionalProperties": false
                        }
                    }
                },
                "required": ["code_review"],
                "additionalProperties": false
            }
            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

            JsonElement reviewsArray = structuredJson.RootElement.GetProperty("code_review");

            JsonElement firstReview = reviewsArray[0];

            return new CodeReviewResult
            {
                IsDoneGood = firstReview.GetProperty("isDoneGood").GetBoolean(),
                Review = firstReview.GetProperty("review").GetString()!
            };
        }

        public async Task<CodeReviewResult> AnalyzeCode(CodeRequest request)
        {
            ChatClient client = new("gpt-4o-mini", apiKey: apiKey);

            List<ChatMessage> messages = new()
            {
        new UserChatMessage($"{request.Code} Czy ten kod jest poprawny i czy masz do niego jakieś rady? Odpowiedz po polsku")
    };

            ChatCompletionOptions options = new()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "code_review",
                    jsonSchema: BinaryData.FromBytes("""
            {
                "type": "object",
                "properties": {
                    "code_review": {
                        "type": "array",
                        "items": {
                            "type": "object",
                            "properties": {
                                "isDoneGood": { "type": "boolean" },
                                "advice": { "type": "string" }
                            },
                            "required": ["isDoneGood", "advice"],
                            "additionalProperties": false
                        }
                    }
                },
                "required": ["code_review"],
                "additionalProperties": false
            }
            """u8.ToArray()),
                    jsonSchemaIsStrict: true
                )
            };

            ChatCompletion completion = await client.CompleteChatAsync(messages, options);

            using JsonDocument structuredJson = JsonDocument.Parse(completion.Content[0].Text);

            JsonElement reviewsArray = structuredJson.RootElement.GetProperty("code_review");

            JsonElement firstReview = reviewsArray[0];

            return new CodeReviewResult
            {
                IsDoneGood = firstReview.GetProperty("isDoneGood").GetBoolean(),
                Review = firstReview.GetProperty("advice").GetString()!
            };
        }
    }
}