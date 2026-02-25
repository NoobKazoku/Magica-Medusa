using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.enums;
using GFramework.Game.Abstractions.ui;
using GFramework.Godot.ui;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using MagicaMedusa.scripts.cqrs.dialogue.command;
using MagicaMedusa.scripts.dialogue;
using Godot;
using MagicaMedusa.scripts.core.ui;
using MagicaMedusa.scripts.enums.ui;

namespace MagicaMedusa.scripts.tests;

/// <summary>
///     对话系统测试脚本
/// </summary>
[ContextAware]
[Log]
public partial class DialogueTest : Control, IController, IUiPageBehaviorProvider, ISimpleUiPage
{
    /// <summary>
    ///     页面行为实例的私有字段
    /// </summary>
    private IUiPageBehavior? _page;
    private Button TestButton1 => GetNode<Button>("%TestButton1");
    private Button TestButton2 => GetNode<Button>("%TestButton2");
    private Button TestButton3 => GetNode<Button>("%TestButton3");
    /// <summary>
    ///     Ui Key的字符串形式
    /// </summary>
    public static string UiKeyStr => nameof(UiKey.DialogueTest);
    public override void _Ready()
    {
        SetupEventHandlers();
    }
    /// <summary>
    ///     获取页面行为实例，如果不存在则创建新的CanvasItemUiPageBehavior实例
    /// </summary>
    /// <returns>返回IUiPageBehavior类型的页面行为实例</returns>
    public IUiPageBehavior GetPage()
    {
        _page ??= UiPageBehaviorFactory.Create<Control>(this, UiKeyStr, UiLayer.Page);
        return _page;
    }

    private void SetupEventHandlers()
    {
        // 测试用例1：简单对话
        TestButton1.Pressed += () =>
        {
            _log.Debug("Test 1: Simple dialogue");
            var sequence = new DialogueSequence
            {
                Dialogues = new List<DialogueData>
                {
                    new DialogueData
                    {
                        SpeakerName = "系统",
                        Text = "欢迎来到对话系统测试！这是一个简单的单句对话。",
                        TypewriterSpeed = 30f
                    }
                }
            };

            this.SendCommand(new StartDialogueCommand { Sequence = sequence });
        };

        // 测试用例2：多句对话
        TestButton2.Pressed += () =>
        {
            _log.Debug("Test 2: Multiple dialogues");
            var sequence = new DialogueSequence
            {
                Dialogues = new List<DialogueData>
                {
                    new DialogueData
                    {
                        SpeakerName = "NPC",
                        Text = "你好，旅行者！欢迎来到这个神秘的世界。",
                        TypewriterSpeed = 30f
                    },
                    new DialogueData
                    {
                        SpeakerName = "玩家",
                        Text = "你好！这里是什么地方？",
                        TypewriterSpeed = 30f
                    },
                    new DialogueData
                    {
                        SpeakerName = "NPC",
                        Text = "这里是魔法美杜莎的领地。小心前行，祝你好运！",
                        TypewriterSpeed = 30f
                    }
                }
            };

            this.SendCommand(new StartDialogueCommand { Sequence = sequence });
        };

        // 测试用例3：带头像对话
        TestButton3.Pressed += () =>
        {
            _log.Debug("Test 3: Dialogue with avatar");
            var sequence = new DialogueSequence
            {
                Dialogues = new List<DialogueData>
                {
                    new DialogueData
                    {
                        SpeakerName = "美杜莎",
                        Text = "凝视我的眼睛，感受石化的力量！",
                        AvatarPath = "res://docs/docs_images/The_snake_emblem.png",
                        TypewriterSpeed = 25f
                    },
                    new DialogueData
                    {
                        SpeakerName = "勇者",
                        Text = "我不会被你的魔法所迷惑！",
                        TypewriterSpeed = 35f
                    }
                }
            };

            this.SendCommand(new StartDialogueCommand { Sequence = sequence });
        };
    }
}
