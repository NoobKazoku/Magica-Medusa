using MagicaMedusa.scripts.enums.resources;
using MagicaMedusa.scripts.enums.scene;

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
    public TextureKey? AvatarTextureKey { get; set; }

    /// <summary>
    ///     打字速度（字符/秒），默认30
    /// </summary>
    public float TypewriterSpeed { get; set; } = 30f;
}

