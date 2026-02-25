using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using MagicaMedusa.scripts.core.audio.system;
using MagicaMedusa.scripts.cqrs.dialogue.command;
using MagicaMedusa.scripts.enums.audio;
using MagicaMedusa.scripts.enums.ui;
using Godot;
using MagicaMedusa.scripts.core.ui;
using MagicaMedusa.scripts.utility;

namespace MagicaMedusa.scripts.dialogue;

/// <summary>
///     对话框UI组件，负责显示对话内容和处理交互
/// </summary>
[ContextAware]
[Log]
public partial class DialogueBox : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    private IUiPageBehavior? _page;
    private DialogueData? _currentDialogue;
    private bool _isTyping;
    private bool _skipTypewriter;
    private float _typewriterTimer;
    private int _visibleCharacters;
    private float _sfxTimer;
    private IAudioSystem? _audioSystem;
    private IGodotTextureRegistry _textureRegistry = null!;

    /// <summary>
    ///     背景面板节点
    /// </summary>
    private Panel BackgroundPanel => GetNode<Panel>("%BackgroundPanel");

    /// <summary>
    ///     说话者名称标签节点
    /// </summary>
    private Label SpeakerNameLabel => GetNode<Label>("%SpeakerNameLabel");

    /// <summary>
    ///     对话文本标签节点
    /// </summary>
    private RichTextLabel DialogueTextLabel => GetNode<RichTextLabel>("%DialogueTextLabel");

    /// <summary>
    ///     头像纹理节点
    /// </summary>
    private TextureRect AvatarTexture => GetNode<TextureRect>("%AvatarTexture");

    /// <summary>
    ///     动画播放器节点
    /// </summary>
    private AnimationPlayer AnimationPlayer => GetNode<AnimationPlayer>("%AnimationPlayer");

    /// <summary>
    ///     UI Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.DialogueBox);

    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<Control>(this, UiKeyStr, UiLayer.Modal);
        return _page;
    }

    public override void _Ready()
    {
        _log.Debug("DialogueBox _Ready called");
        _audioSystem = this.GetSystem<IAudioSystem>();
        _textureRegistry = this.GetUtility<IGodotTextureRegistry>()!;
        Visible = false;
        DialogueTextLabel.Text = string.Empty;
        SpeakerNameLabel.Text = string.Empty;
    }

    public override void _Process(double delta)
    {
        if (!_isTyping || _currentDialogue == null)
            return;

        // 如果跳过打字机效果，立即显示全文
        if (_skipTypewriter)
        {
            DialogueTextLabel.VisibleCharacters = -1;
            _isTyping = false;
            _skipTypewriter = false;
            return;
        }

        // 更新打字机效果
        _typewriterTimer += (float)delta;
        var charsPerSecond = _currentDialogue.TypewriterSpeed;
        var targetChars = (int)(_typewriterTimer * charsPerSecond);

        if (targetChars > _visibleCharacters)
        {
            _visibleCharacters = targetChars;
            DialogueTextLabel.VisibleCharacters = _visibleCharacters;

            // 播放打字音效
            _sfxTimer += (float)delta;
            if (_sfxTimer >= 0.05f)
            {
                _audioSystem?.PlaySfx(SfxType.DialogueTypewriter);
                _sfxTimer = 0f;
            }
        }

        // 检查是否打字完成
        if (DialogueTextLabel.VisibleCharacters >= DialogueTextLabel.GetTotalCharacterCount())
        {
            _isTyping = false;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (!Visible)
            return;

        // 检测鼠标点击或确认键
        if (@event is not InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } &&
            !@event.IsActionPressed("ui_accept")) return;
        if (_isTyping)
        {
            // 打字中，跳过打字机效果
            _skipTypewriter = true;
        }
        else
        {
            // 打字完成，推进对话
            this.SendCommand(new AdvanceDialogueCommand());
        }

        AcceptEvent();
    }

    /// <summary>
    ///     显示对话框并播放弹入动画
    /// </summary>
    public async Task ShowAsync()
    {
        _log.Debug("ShowAsync: Starting to show dialogue box");
        Visible = true;

        // 检查动画播放器是否有动画
        if (!AnimationPlayer.HasAnimation("slide_in"))
        {
            _log.Error("ShowAsync: Animation 'slide_in' not found!");
            return;
        }

        _log.Debug("ShowAsync: Playing slide_in animation");
        AnimationPlayer.Play("slide_in");

        _log.Debug("ShowAsync: Waiting for animation to finish");
        await ToSignal(AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        _log.Debug("ShowAsync: Animation finished");
    }

    /// <summary>
    ///     隐藏对话框并播放退出动画
    /// </summary>
    public async Task HideAsync()
    {
        AnimationPlayer.Play("slide_out");
        await ToSignal(AnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        Visible = false;
    }

    /// <summary>
    ///     设置对话内容并启动打字机效果
    /// </summary>
    /// <param name="dialogue">对话数据</param>
    public void SetDialogue(DialogueData dialogue)
    {
        _log.Debug($"SetDialogue: Speaker={dialogue.SpeakerName}, Text={dialogue.Text}");
        _currentDialogue = dialogue;

        // 设置说话者名称
        SpeakerNameLabel.Text = dialogue.SpeakerName;

        // 设置对话文本
        DialogueTextLabel.Text = dialogue.Text;
        DialogueTextLabel.VisibleCharacters = 0;

        // 设置头像
        if (dialogue.AvatarTextureKey is not null)
        {
            var textureKey = dialogue.AvatarTextureKey.Value;
            _log.Debug($"SetDialogue: Loading avatar texture with key={textureKey}");

            if (_textureRegistry.Get(textureKey.ToString()) is Texture2D texture)
            {
                AvatarTexture.Texture = texture;
                AvatarTexture.Visible = true;
                _log.Debug("SetDialogue: Avatar texture loaded successfully");
            }
            else
            {
                AvatarTexture.Visible = false;
                _log.Warn($"Failed to load avatar texture: {textureKey}");
            }
        }
        else
        {
            AvatarTexture.Visible = false;
        }

        // 启动打字机效果
        _isTyping = true;
        _skipTypewriter = false;
        _typewriterTimer = 0f;
        _visibleCharacters = 0;
        _sfxTimer = 0f;
        _log.Debug("SetDialogue: Typewriter effect started");
    }
}
