using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;

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
            new ChatMessage(ChatRole.User, prompt),
        };

        var response = await client.GetResponseAsync(chatMessages);
        return response.Text;
    }
}
