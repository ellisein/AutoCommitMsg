using AutoCommitMsg.Models.ChatResponses;
using AutoCommitMsg.ViewModels;
using Microsoft.Extensions.AI;
using Newtonsoft.Json.Schema.Generation;
using OpenAI;
using System.ClientModel;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace AutoCommitMsg.Services;

public static class AiService
{
    public static IChatClient ChatClient()
    {
        var credential = new ApiKeyCredential(EmbeddedApiKey.OpenAiApiKey);
        var openaiClient = new OpenAIClient(credential)
            .GetChatClient("o4-mini")
            .AsIChatClient();
        return new ChatClientBuilder(openaiClient)
            .UseFunctionInvocation()
            .Build();
    }

    public static async Task<CommitMessages?> GenerateCommitMessagesAsync(List<string> gitLogs, string gitDiff, AppLanguage language)
    {
        var prompt = LoadPrompt("generate-commit-messages");
        var formattedLogs = string.Join("\n", gitLogs);
        prompt = prompt
            .Replace("{git_logs}", formattedLogs)
            .Replace("{git_diff}", gitDiff)
            .Replace("{language}", language.ToString());

        var generator = new JSchemaGenerator();
        var schema = generator.Generate(typeof(CommitMessages));
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(schema.ToString());
        var chatResponseFormat = ChatResponseFormat.ForJsonSchema(jsonElement);

        using var client = AiService.ChatClient();
        var response = await client.GetResponseAsync(prompt, new ChatOptions { ResponseFormat = chatResponseFormat });

        return JsonSerializer.Deserialize<CommitMessages>(response.Text);
    }

    private static string LoadPrompt(string promptName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"AutoCommitMsg.Resources.Prompts.{promptName}.txt";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
