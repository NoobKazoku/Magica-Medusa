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
using GFramework.SourceGenerators.Abstractions.logging;
using Godot;
using MagicaMedusa.scripts.core.controller;
using MagicaMedusa.scripts.ecs.components;
using MagicaMedusa.scripts.enums;

namespace MagicaMedusa.scripts.tests;

/// <summary>
///     玩家输入控制器，负责轮询玩家输入并写入 ECS 组件
/// </summary>
[Log]
public partial class PlayerInputController : GameInputController
{
    /// <summary>
    ///     玩家实体引用
    /// </summary>
    private Entity _playerEntity;

    /// <summary>
    ///     ECS World 引用
    /// </summary>
    private World _world;

    /// <summary>
    ///     初始化输入控制器
    /// </summary>
    /// <param name="world">ECS World 实例</param>
    /// <param name="playerEntity">玩家实体</param>
    public void Initialize(World world, Entity playerEntity)
    {
        _world = world;
        _playerEntity = playerEntity;
    }

    /// <summary>
    ///     接受 Gameplay 阶段的输入
    /// </summary>
    protected override bool AcceptPhase(InputPhase phase)
    {
        _log.Debug($"输入状态：{phase.ToString()}");
        return phase == InputPhase.Gameplay;
    }

    /// <summary>
    ///     处理输入事件（本控制器使用轮询方式，此方法留空）
    /// </summary>
    protected override void Handle(InputPhase phase, InputEvent @event)
    {
        // 使用 _Process 轮询输入，此方法不需要实现
    }

    /// <summary>
    ///     每帧轮询输入并更新 ECS 组件
    /// </summary>
    public override void _Process(double delta)
    {
        if (!_world.IsAlive(_playerEntity))
            return;
        
        // 获取水平输入（A/D 或 左箭头/右箭头）
        var horizontalInput = Input.GetAxis("move_left", "move_right");

        // 更新输入方向组件
        ref var inputDirection = ref _world.Get<InputDirection>(_playerEntity);
        inputDirection.X = horizontalInput;

        // 获取跳跃输入（空格 或 W）
        var jumpPressed = Input.IsActionJustPressed("jump");
        var jumpHeld = Input.IsActionPressed("jump");

        // 更新跳跃输入组件
        ref var jumpInput = ref _world.Get<JumpInput>(_playerEntity);
        jumpInput.JumpPressed = jumpPressed;
        jumpInput.JumpHeld = jumpHeld;
    }
}
