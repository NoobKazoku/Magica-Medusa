namespace MagicaMedusa.scripts.dialogue;

/// <summary>
///     对话数据模型，表示单条对话内容
/// </summary>
public class DialogueData
{
    /// <summary>
    ///     说话者名称
    /// </summary>
    public string SpeakerName { get; set; } = string.Empty;

    /// <summary>
    ///     对话文本内容
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    ///     头像路径（可选）
    /// </summary>
    public string? AvatarPath { get; set; }

    /// <summary>
    ///     打字速度（字符/秒），默认30
    /// </summary>
    public float TypewriterSpeed { get; set; } = 30f;
}

/// <summary>
///     对话序列，包含一组连续的对话
/// </summary>
public class DialogueSequence
{
    /// <summary>
    ///     对话列表
    /// </summary>
    public List<DialogueData> Dialogues { get; set; } = new();

    /// <summary>
    ///     是否在对话期间暂停游戏，默认true
    /// </summary>
    public bool PauseGameDuringDialogue { get; set; } = true;

    /// <summary>
    ///     完成回调ID（可选）
    /// </summary>
    public string? OnCompleteCallbackId { get; set; }
}
