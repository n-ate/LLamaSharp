using LlmToolkit.Common;
using static LlmToolkit.LLamaTransforms;

namespace LLamaSharp.SemanticKernel.ChatCompletion;

/// <summary>
/// Default HistoryTransform Patch
/// </summary>
public class HistoryTransform : DefaultHistoryTransform
{
    /// <inheritdoc/>
    public override string HistoryToText(ChatHistory history)
    {
        return base.HistoryToText(history) + $"{AuthorRole.Assistant}: ";
    }
}
