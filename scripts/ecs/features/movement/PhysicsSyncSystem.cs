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
using GFramework.Core.ecs;
using Godot;
using MagicaMedusa.scripts.ecs.components;

namespace MagicaMedusa.scripts.ecs.features.movement;

/// <summary>
///     物理同步系统类，作为 ECS 与 Godot 物理引擎的桥梁
///     负责将 ECS 的速度数据同步到 Godot 物理体，并回读物理模拟结果
/// </summary>
public class PhysicsSyncSystem : ArchSystemAdapter<float>
{
    /// <summary>
    ///     ECS 查询描述符，用于筛选具有物理体、速度和跳跃状态组件的实体
    /// </summary>
    private QueryDescription _query;

    /// <summary>
    ///     系统初始化方法，设置 ECS 查询条件
    /// </summary>
    protected override void OnArchInitialize()
    {
        _query = new QueryDescription()
            .WithAll<PhysicsBody, Velocity, JumpState>();
    }

    /// <summary>
    ///     系统更新方法，每帧调用以同步物理状态
    /// </summary>
    /// <param name="t">时间增量（秒）</param>
    protected override void OnUpdate(in float t)
    {
        World.Query(in _query, (ref PhysicsBody physicsBody, ref Velocity vel, ref JumpState jumpState) =>
        {
            // 从实例 ID 获取 CharacterBody2D 节点
            var body = GodotObject.InstanceFromId(physicsBody.NodeId) as CharacterBody2D;
            if (body == null) return;

            // 将 ECS 速度写入 Godot 物理体
            body.Velocity = new Vector2(vel.X, vel.Y);

            // 执行物理模拟（碰撞检测与响应）
            body.MoveAndSlide();

            // 回读实际速度（可能被碰撞修改）
            vel.X = body.Velocity.X;
            vel.Y = body.Velocity.Y;

            // 更新地面状态
            var wasGrounded = jumpState.IsGrounded;
            jumpState.IsGrounded = body.IsOnFloor();

            // 更新土狼时间计数器
            if (jumpState.IsGrounded)
            {
                jumpState.CoyoteFrames = 0;
            }
            else if (wasGrounded && !jumpState.IsGrounded)
            {
                // 刚离开地面，开始计数
                jumpState.CoyoteFrames = 1;
            }
            else if (!jumpState.IsGrounded)
            {
                // 持续在空中，递增计数
                jumpState.CoyoteFrames++;
            }
        });
    }
}
