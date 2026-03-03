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
///     重力系统类，负责对实体应用重力效果
///     通过查询具有速度和跳跃状态组件的实体，模拟重力加速度
/// </summary>
public class GravitySystem : ArchSystemAdapter<float>
{
    /// <summary>
    ///     ECS 查询描述符，用于筛选具有速度和跳跃状态组件的实体
    /// </summary>
    private QueryDescription _query;

    /// <summary>
    ///     系统初始化方法，设置 ECS 查询条件
    /// </summary>
    protected override void OnArchInitialize()
    {
        _query = new QueryDescription()
            .WithAll<Velocity, JumpState>();
    }

    /// <summary>
    ///     系统更新方法，每帧调用以应用重力
    /// </summary>
    /// <param name="t">时间增量（秒）</param>
    protected override void OnUpdate(in float t)
    {
        var deltaTime = t;
        World.Query(in _query, (ref Velocity vel, ref JumpState jumpState) =>
        {
            // 当不在地面或向上移动时，应用重力
            if (!jumpState.IsGrounded || vel.Y < 0)
            {
                vel.Y += jumpState.Gravity * deltaTime;

                // 限制最大下落速度
                if (vel.Y > jumpState.MaxFallSpeed)
                {
                    vel.Y = jumpState.MaxFallSpeed;
                }
            }
        });
    }
}
