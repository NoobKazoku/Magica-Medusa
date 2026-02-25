using GFramework.Core.cqrs.command;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.coroutine;
using MagicaMedusa.scripts.core.utils;
using MagicaMedusa.scripts.cqrs.game.command;
using MagicaMedusa.scripts.dialogue;
using MagicaMedusa.scripts.enums.ui;
using Mediator;
using Unit = Mediator.Unit;

namespace MagicaMedusa.scripts.cqrs.dialogue.command;

/// <summary>
///     开始对话命令处理器类，负责处理开始对话的命令逻辑
/// </summary>
public class StartDialogueCommandHandler : AbstractCommandHandler<StartDialogueCommand>
{
    private IUiRouter? _uiRouter;
    private DialogueManager? _dialogueManager;

    /// <summary>
    ///     处理开始对话命令的核心方法
    /// </summary>
    /// <param name="command">开始对话命令对象</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>表示异步操作完成的ValueTask</returns>
    public override ValueTask<Unit> Handle(StartDialogueCommand command, CancellationToken cancellationToken)
    {
        _uiRouter ??= this.GetSystem<IUiRouter>();
        _dialogueManager ??= this.GetSystem<DialogueManager>();

        var sequence = command.Sequence;

        // 如果需要暂停游戏
        if (sequence.PauseGameDuringDialogue)
        {
            GameUtil.GetTree().Paused = true;
        }

        // 打开对话框UI并开始对话
        StartDialogueAsync(sequence).ToCoroutineEnumerator().RunCoroutine();

        return ValueTask.FromResult(Unit.Value);
    }

    private async Task StartDialogueAsync(DialogueSequence sequence)
    {
        // 打开对话框UI
        await _uiRouter!.PushAsync(nameof(UiKey.DialogueBox)).ConfigureAwait(true);

        // 获取对话框实例
        if (_uiRouter.Peek()!.View is not DialogueBox dialogueBox)
        {
            return;
        }

        // 开始对话序列
        await _dialogueManager!.StartDialogueAsync(sequence, dialogueBox).ConfigureAwait(true);
    }
}
