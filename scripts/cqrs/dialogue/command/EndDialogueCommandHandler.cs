using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using MagicaMedusa.scripts.core.utils;
using MagicaMedusa.scripts.cqrs.game.command;
using MagicaMedusa.scripts.dialogue;
using Mediator;
using Unit = Mediator.Unit;

namespace MagicaMedusa.scripts.cqrs.dialogue.command;

/// <summary>
///     结束对话命令处理器类，负责处理结束对话的命令逻辑
/// </summary>
public class EndDialogueCommandHandler : AbstractCommandHandler<EndDialogueCommand>
{
    private DialogueManager? _dialogueManager;
    private IUiRouter? _uiRouter;

    /// <summary>
    ///     处理结束对话命令的核心方法
    /// </summary>
    /// <param name="command">结束对话命令对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步操作完成的ValueTask</returns>
    public override ValueTask<Unit> Handle(EndDialogueCommand command, CancellationToken cancellationToken)
    {
        _dialogueManager ??= this.GetSystem<DialogueManager>();
        _uiRouter ??= this.GetSystem<IUiRouter>();

        EndDialogueAsync().ToCoroutineEnumerator().RunCoroutine();

        return ValueTask.FromResult(Unit.Value);
    }

    private async Task EndDialogueAsync()
    {
        // 结束对话
        await _dialogueManager!.EndDialogueAsync().ConfigureAwait(true);

        // 关闭对话框UI
        await _uiRouter!.PopAsync().ConfigureAwait(true);

        // 恢复游戏
        GameUtil.GetTree().Paused = false;
    }
}
