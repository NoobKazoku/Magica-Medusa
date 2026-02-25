using GFramework.Core.Abstractions.controller;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using MagicaMedusa.scripts.cqrs.dialogue.command;
using Godot;
using Godot.Collections;
using MagicaMedusa.scripts.enums.resources;
using Array = Godot.Collections.Array;

namespace MagicaMedusa.scripts.dialogue;

/// <summary>
///     对话触发器组件，通过Area2D触发对话
/// </summary>
[ContextAware]
[Log]
public partial class DialogueTrigger : Area2D, IController
{
    private bool _hasTriggered;

    /// <summary>
    ///     说话者名称数组
    /// </summary>
    [Export]
    public string[] SpeakerNames { get; set; } = [];

    /// <summary>
    ///     对话文本数组
    /// </summary>
    [Export(PropertyHint.MultilineText)]
    public string[] DialogueTexts { get; set; } = [];

    /// <summary>
    ///     头像路径数组（可选）
    /// </summary>
    [Export]
    public Array<TextureKey> AvatarTextureKeys { get; set; }=[];

    /// <summary>
    ///     是否只触发一次
    /// </summary>
    [Export]
    public bool TriggerOnce { get; set; } = true;

    /// <summary>
    ///     是否在对话期间暂停游戏
    /// </summary>
    [Export]
    public bool PauseGameDuringDialogue { get; set; } = true;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    /// <summary>
    ///     当有物体进入触发区域时调用
    /// </summary>
    private void OnBodyEntered(Node2D body)
    {
        // 检查是否已触发且设置为只触发一次
        if (_hasTriggered && TriggerOnce)
            return;

        // 检查进入的是否是玩家
        if (!body.IsInGroup("player"))
            return;

        // 验证数据有效性
        if (SpeakerNames.Length == 0 || DialogueTexts.Length == 0)
        {
            _log.Warn("DialogueTrigger: SpeakerNames or DialogueTexts is empty");
            return;
        }

        if (SpeakerNames.Length != DialogueTexts.Length)
        {
            _log.Warn("DialogueTrigger: SpeakerNames and DialogueTexts length mismatch");
            return;
        }

        // 构建对话序列
        var sequence = new DialogueSequence
        {
            PauseGameDuringDialogue = PauseGameDuringDialogue,
        };

        for (var i = 0; i < DialogueTexts.Length; i++)
        {
            var dialogue = new DialogueData
            {
                SpeakerName = SpeakerNames[i],
                Text = DialogueTexts[i]
            };

            // 如果有头像路径，设置头像
            if (AvatarTextureKeys.Count > i)
            {
                dialogue.AvatarTextureKey = AvatarTextureKeys[i];
            }

            sequence.Dialogues.Add(dialogue);
        }

        // 发送开始对话命令
        this.SendCommand(new StartDialogueCommand
        {
            Sequence = sequence
        });

        _hasTriggered = true;
        _log.Debug($"DialogueTrigger: Started dialogue with {sequence.Dialogues.Count} dialogues");
    }

    /// <summary>
    ///     重置触发状态（用于可重复触发的情况）
    /// </summary>
    public void ResetTrigger()
    {
        _hasTriggered = false;
    }
}
