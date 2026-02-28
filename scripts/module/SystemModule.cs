using GFramework.Core.Abstractions.architecture;
using GFramework.Game.setting;
using MagicaMedusa.scripts.core.audio.system;
using MagicaMedusa.scripts.core.scene;
using MagicaMedusa.scripts.core.ui;
using MagicaMedusa.scripts.dialogue;

namespace MagicaMedusa.scripts.module;

/// <summary>
///     系统Godot模块类，负责安装和注册游戏所需的各种系统组件
///     继承自AbstractGodotModule，用于在游戏架构中集成系统功能
/// </summary>
public class SystemModule : IArchitectureModule
{
    /// <summary>
    ///     安装方法，用于向游戏架构注册各种系统组件
    /// </summary>
    /// <param name="architecture">游戏架构接口实例，用于注册系统</param>
    public void Install(IArchitecture architecture)
    {
        architecture.RegisterSystem(new UiRouter());
        architecture.RegisterSystem(new SceneRouter());
        architecture.RegisterSystem(new SettingsSystem());
        architecture.RegisterSystem(new GodotAudioSystem());
        architecture.RegisterSystem(new DialogueManager());
    }
}