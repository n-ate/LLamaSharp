using LlmToolkit.Common;
using LlmToolkit.Sampling;
using chatHist = Microsoft.SemanticKernel.ChatCompletion.ChatHistory;
using AuthorRole = LlmToolkit.Common.AuthorRole;

namespace LLamaSharp.SemanticKernel;

public static class ExtensionMethods
{
    public static ChatHistory ToLLamaSharpChatHistory(this chatHist chatHistory, bool ignoreCase = true)
    {
        if (chatHistory is null)
        {
            throw new ArgumentNullException(nameof(chatHistory));
        }

        var history = new ChatHistory();

        foreach (var chat in chatHistory)
        {
            if (!Enum.TryParse<AuthorRole>(chat.Role.Label, ignoreCase, out var role))
                role = AuthorRole.Unknown;

            history.AddMessage(role, chat.Content ?? "");
        }

        return history;
    }

    /// <summary>
    /// Convert LLamaSharpPromptExecutionSettings to LLamaSharp InferenceParams
    /// </summary>
    /// <param name="requestSettings"></param>
    /// <returns></returns>
    internal static InferenceParams ToLLamaSharpInferenceParams(this LLamaSharpPromptExecutionSettings requestSettings)
    {
        if (requestSettings is null)
        {
            throw new ArgumentNullException(nameof(requestSettings));
        }

        var antiPrompts = new List<string>(requestSettings.StopSequences)
        {
            $"{AuthorRole.User}:",
            $"{AuthorRole.Assistant}:",
            $"{AuthorRole.System}:"
        };
        return new InferenceParams
        {
            AntiPrompts = antiPrompts,
            MaxTokens = requestSettings.MaxTokens ?? -1,

            SamplingPipeline = new DefaultSamplingPipeline()
            {
                Temperature = (float)requestSettings.Temperature,
                TopP = (float)requestSettings.TopP,
                PresencePenalty = (float)requestSettings.PresencePenalty,
                FrequencyPenalty = (float)requestSettings.FrequencyPenalty,
            }
        };
    }
}
