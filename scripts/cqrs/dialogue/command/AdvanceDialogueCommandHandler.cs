using GFramework.Core.cqrs.command;
using GFramework.Godot.coroutine;
using MagicaMedusa.scripts.dialogue;
using Mediator;
using Unit = Mediator.Unit;

namespace MagicaMedusa.scripts.cqrs.dialogue.command;

/// <summary>
///     推进对话命令处理器类，负责处理推进对话的命令逻辑
/// </summary>
public class AdvanceDialogueCommandHandler : AbstractCommandHandler<AdvanceDialogueCommand>
{
    private DialogueManager? _dialogueManager;

    /// <summary>
    ///     处理推进对话命令的核心方法
    /// </summary>
    /// <param name="command">推进对话命令对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步操作完成的ValueTask</returns>
    public override ValueTask<Unit> Handle(AdvanceDialogueCommand command, CancellationToken cancellationToken)
    {
        _dialogueManager ??= this.GetSystem<DialogueManager>();

        AdvanceDialogueAsync().ToCoroutineEnumerator().RunCoroutine();

        return ValueTask.FromResult(Unit.Value);
    }

    private async Task AdvanceDialogueAsync()
    {
        var hasMore = await _dialogueManager!.AdvanceDialogueAsync().ConfigureAwait(false);

        // 如果没有更多对话，结束对话
        if (!hasMore)
        {
            await this.SendCommandAsync(new EndDialogueCommand()).ConfigureAwait(false);
        }
    }
}
