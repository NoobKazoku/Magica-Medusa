// Copyright (c) 2026 GeWuYou
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Arch.Core;
using GFramework.Core.Abstractions.controller;
using GFramework.Game.Abstractions.scene;
using GFramework.Godot.scene;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;
using MagicaMedusa.scripts.core.scene;
using MagicaMedusa.scripts.ecs.components;
using MagicaMedusa.scripts.enums.scene;

namespace MagicaMedusa.scripts.tests;

/// <summary>
///     玩家测试场景控制器
///     用于测试 ECS 移动系统和玩家控制
/// </summary>
[ContextAware]
[Log]
public partial class PlayerTest : Node2D, IController, ISceneBehaviorProvider, ISimpleScene
{
    /// <summary>
    ///     玩家 CharacterBody2D 节点（在编辑器中关联）
    /// </summary>
    [Export] public CharacterBody2D Player;

    /// <summary>
    ///     场景行为实例
    /// </summary>
    private ISceneBehavior? _scene;

    /// <summary>
    ///     玩家 ECS 实体
    /// </summary>
    private Entity _playerEntity;

    /// <summary>
    ///     ECS World 实例
    /// </summary>
    private World _world = null!;

    /// <summary>
    ///     输入控制器实例
    /// </summary>
    private PlayerInputController InputController => GetNode<PlayerInputController>("%PlayerInputController");

    /// <summary>
    ///     场景键值字符串
    /// </summary>
    public static string SceneKeyStr => nameof(SceneKey.PlayerTest);

    /// <summary>
    ///     场景初始化
    /// </summary>
    public override void _Ready()
    {
        // 获取 ECS World 实例
        _world = this.GetService<World>()!;

        // 创建玩家 ECS 实体并附加所有必需组件
        _playerEntity = _world.Create(
            new InputDirection { X = 0, Y = 0 },
            new MoveSpeed { Speed = 200f, Friction = 800f },
            new Velocity { X = 0, Y = 0 },
            new Facing { FacingRight = true },
            new JumpInput { JumpPressed = false, JumpHeld = false },
            new JumpState
            {
                JumpForce = 400f,
                Gravity = 980f,
                MaxFallSpeed = 500f,
                IsGrounded = false,
                CoyoteFrames = 0,
                JumpBufferFrames = 999 // 初始化为大值，表示没有跳跃缓冲
            },
            new PhysicsBody { NodeId = Player.GetInstanceId() }
        );
        InputController.Initialize(_playerEntity);
    }

    /// <summary>
    ///     场景退出时清理
    /// </summary>
    public override void _ExitTree()
    {
        // 销毁玩家 ECS 实体
        if (_world.IsAlive(_playerEntity))
        {
            _world.Destroy(_playerEntity);
        }
    }

    /// <summary>
    ///     获取场景行为实例
    /// </summary>
    public ISceneBehavior GetScene()
    {
        _scene ??= SceneBehaviorFactory.Create<Node2D>(this, SceneKeyStr);
        return _scene;
    }
}
