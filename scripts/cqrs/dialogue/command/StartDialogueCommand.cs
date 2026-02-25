using Mediator;
using MagicaMedusa.scripts.dialogue;

namespace MagicaMedusa.scripts.cqrs.dialogue.command;

/// <summary>
///     开始对话命令类，用于启动对话序列
/// </summary>
public class StartDialogueCommand : ICommand
{
    /// <summary>
    ///     对话序列数据
    /// </summary>
    public DialogueSequence Sequence { get; init; } = null!;
}
