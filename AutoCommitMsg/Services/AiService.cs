using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.IO;
using System.Reflection;

namespace AutoCommitMsg.Services;

public static class AiService
{
    public static IChatClient ChatClient()
    {
        var credential = new ApiKeyCredential(EmbeddedApiKey.OpenAiApiKey);
        var openaiClient = new OpenAIClient(credential)
            .GetChatClient("gpt-4o-mini")
            .AsIChatClient();
        return new ChatClientBuilder(openaiClient)
            .UseFunctionInvocation()
            .Build();
    }

    public static async Task<string> RunPromptAsync(string prompt)
    {
        using var client = AiService.ChatClient();

        var chatMessages = new List<ChatMessage>
        {
            new (ChatRole.User, prompt),
        };

        var response = await client.GetResponseAsync(chatMessages);
        return response.Text;
    }

    public static async Task<string> GenerateCommitMessagesAsync(List<string> gitLogs, string gitDiff)
    {
        var prompt = LoadPrompt("generate-commit-messages");
        var formattedLogs = string.Join("\n", gitLogs);
        prompt = prompt
            .Replace("{git_logs}", formattedLogs)
            .Replace("{git_diff}", gitDiff);
        return await RunPromptAsync(prompt);
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
