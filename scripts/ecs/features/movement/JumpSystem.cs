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
using MagicaMedusa.scripts.ecs.components;

namespace MagicaMedusa.scripts.ecs.features.movement;

/// <summary>
///     跳跃系统类，负责处理实体的跳跃逻辑
///     实现土狼时间（Coyote Time）机制，提升操作手感
/// </summary>
public class JumpSystem : ArchSystemAdapter<float>
{
    /// <summary>
    ///     土狼时间的最大帧数（离开地面后仍可跳跃的缓冲帧数）
    /// </summary>
    private const int MaxCoyoteFrames = 6;

    /// <summary>
    ///     跳跃缓冲的最大帧数（按下跳跃键后的缓冲时间）
    /// </summary>
    private const int MaxJumpBufferFrames = 6;

    /// <summary>
    ///     ECS 查询描述符，用于筛选具有跳跃输入、跳跃状态和速度组件的实体
    /// </summary>
    private QueryDescription _query;

    /// <summary>
    ///     系统初始化方法，设置 ECS 查询条件
    /// </summary>
    protected override void OnArchInitialize()
    {
        _query = new QueryDescription()
            .WithAll<JumpInput, JumpState, Velocity>();
    }

    /// <summary>
    ///     系统更新方法，每帧调用以处理跳跃逻辑
    /// </summary>
    /// <param name="t">时间增量（秒）</param>
    protected override void OnUpdate(in float t)
    {
        World.Query(in _query, (ref JumpInput input, ref JumpState jumpState, ref Velocity vel) =>
        {
            // 如果按下跳跃键，重置跳跃缓冲计数器
            if (input.JumpPressed)
            {
                jumpState.JumpBufferFrames = 0;
            }
            else if (jumpState.JumpBufferFrames < MaxJumpBufferFrames)
            {
                // 递增跳跃缓冲计数器
                jumpState.JumpBufferFrames++;
            }

            // 检查是否可以跳跃（在地面或土狼时间内）
            var canJump = jumpState.IsGrounded || jumpState.CoyoteFrames < MaxCoyoteFrames;

            // 检查是否有有效的跳跃缓冲
            var hasJumpBuffer = jumpState.JumpBufferFrames < MaxJumpBufferFrames;

            // 处理跳跃输入
            if (hasJumpBuffer && canJump)
            {
                vel.Y = -jumpState.JumpForce;
                jumpState.CoyoteFrames = MaxCoyoteFrames; // 跳跃后重置土狼时间
                jumpState.JumpBufferFrames = MaxJumpBufferFrames; // 消耗跳跃缓冲
            }

            // 清除本帧的跳跃按下标志
            input.JumpPressed = false;
        });
    }
}
