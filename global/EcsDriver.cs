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
using Arch.System;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;
using MagicaMedusa.scripts.ecs.features.movement;

namespace MagicaMedusa.global;

/// <summary>
///     ECS 驱动器，负责在每个物理帧更新所有 ECS 系统
/// </summary>
[Log]
[ContextAware]
public partial class EcsDriver : Node
{
    /// <summary>
    ///     ECS World 实例
    /// </summary>
    private World _world = null!;

    /// <summary>
    ///     所有需要更新的 ECS 系统
    /// </summary>
    private ISystem<float>[] _systems = null!;

    /// <summary>
    ///     节点就绪时初始化
    /// </summary>
    public override void _Ready()
    {
        // 获取 ECS World 实例
        _world = this.GetService<World>()!;
        // 获取所有注册的 ECS 系统（按正确顺序）
        _systems =
        [
            this.GetSystem<MovementSystem>()!,      // 1. 水平移动
            this.GetSystem<GravitySystem>()!,       // 2. 重力
            this.GetSystem<JumpSystem>()!,          // 3. 跳跃
            this.GetSystem<PhysicsSyncSystem>()!,   // 4. 物理同步
            this.GetSystem<FacingSystem>()!         // 5. 面向方向
        ];

        _log.Info($"ECS Driver initialized with {_systems.Length} systems");
    }

    /// <summary>
    ///     在物理帧更新所有 ECS 系统
    /// </summary>
    /// <param name="delta">物理帧时间增量（秒）</param>
    public override void _PhysicsProcess(double delta)
    {
        var deltaTime = (float)delta;

        // 按顺序更新所有 ECS 系统
        foreach (var system in _systems)
        {
            system.Update(deltaTime);
        }
    }
}
