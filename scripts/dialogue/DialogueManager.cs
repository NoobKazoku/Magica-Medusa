using GFramework.Core.system;
using GFramework.SourceGenerators.Abstractions.logging;
using MagicaMedusa.scripts.core.utils;

namespace MagicaMedusa.scripts.dialogue;

/// <summary>
///     对话管理器系统，负责管理对话序列和状态
/// </summary>
[Log]
public partial class DialogueManager : AbstractSystem
{
    private DialogueSequence? _currentSequence;
    private int _currentIndex;
    private DialogueBox? _dialogueBox;
    private bool _didPauseGame; // 追踪是否由对话系统暂停的游戏

    /// <summary>
    ///     开始对话序列
    /// </summary>
    /// <param name="sequence">对话序列</param>
    /// <param name="dialogueBox">对话框实例</param>
    public async Task StartDialogueAsync(DialogueSequence? sequence, DialogueBox dialogueBox)
    {
        if (sequence == null || sequence.Dialogues.Count == 0)
        {
            _log.Warn("Cannot start dialogue: sequence is null or empty");
            return;
        }

        _currentSequence = sequence;
        _currentIndex = 0;
        _dialogueBox = dialogueBox;

        // 记录是否由对话系统暂停游戏（只有在游戏未暂停且配置要求暂停时才记录）
        _didPauseGame = sequence.PauseGameDuringDialogue && !GameUtil.GetTree().Paused;

        _log.Debug($"Starting dialogue sequence with {sequence.Dialogues.Count} dialogues, didPauseGame={_didPauseGame}");

        // 显示第一句对话
        await ShowCurrentDialogueAsync().ConfigureAwait(true);
    }

    /// <summary>
    ///     推进到下一句对话
    /// </summary>
    /// <returns>是否还有更多对话</returns>
    public async Task<bool> AdvanceDialogueAsync()
    {
        if (_currentSequence == null || _dialogueBox == null)
        {
            _log.Warn("Cannot advance dialogue: no active dialogue sequence");
            return false;
        }

        _currentIndex++;

        if (_currentIndex >= _currentSequence.Dialogues.Count)
        {
            _log.Debug("Dialogue sequence completed");
            return false;
        }

        _log.Debug($"Advancing to dialogue {_currentIndex + 1}/{_currentSequence.Dialogues.Count}");
        await ShowCurrentDialogueAsync().ConfigureAwait(true);
        return true;
    }

    /// <summary>
    ///     结束对话并清理状态
    /// </summary>
    public async Task EndDialogueAsync()
    {
        if (_dialogueBox != null)
        {
            _log.Debug("Ending dialogue sequence");
            await _dialogueBox.HideAsync().ConfigureAwait(true);
        }

        _currentSequence = null;
        _currentIndex = 0;
        _dialogueBox = null;
        // 注意：不在这里重置 _didPauseGame，由 EndDialogueCommandHandler 使用后重置
    }

    /// <summary>
    ///     检查是否应该恢复游戏（只有对话系统暂停的才恢复）
    /// </summary>
    public bool ShouldResumeGame()
    {
        var shouldResume = _didPauseGame;
        _didPauseGame = false; // 重置标志
        return shouldResume;
    }

    /// <summary>
    ///     显示当前对话
    /// </summary>
    private async Task ShowCurrentDialogueAsync()
    {
        if (_currentSequence == null || _dialogueBox == null)
            return;

        _log.Debug($"ShowCurrentDialogueAsync: index={_currentIndex}");
        var dialogue = _currentSequence.Dialogues[_currentIndex];

        // 如果是第一句对话，需要显示对话框
        if (_currentIndex == 0)
        {
            _log.Debug("ShowCurrentDialogueAsync: Showing dialogue box");
            await _dialogueBox.ShowAsync().ConfigureAwait(true);
            _log.Debug("ShowCurrentDialogueAsync: Dialogue box shown");
        }

        _log.Debug("ShowCurrentDialogueAsync: Setting dialogue content");
        _dialogueBox.SetDialogue(dialogue);
        _log.Debug("ShowCurrentDialogueAsync: Dialogue content set");
    }

    protected override void OnInit()
    {
        
    }
}
