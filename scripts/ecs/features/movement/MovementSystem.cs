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
///     移动系统类，负责处理游戏中实体的移动逻辑
///     通过查询具有输入方向、移动速度和速度组件的实体，计算并更新其实时速度
/// </summary>
public class MovementSystem : ArchSystemAdapter<float>
{
    /// <summary>
    ///     浮点数比较的容差值，用于避免浮点数精度问题
    /// </summary>
    private const float Epsilon = 0.0001f;
    
    /// <summary>
    ///     ECS查询描述符，用于筛选包含特定组件的实体
    /// </summary>
    private QueryDescription _query;

    protected override void OnArchInitialize()
    {
        _query = new QueryDescription()
            .WithAll<InputDirection, MoveSpeed, Velocity>();
    }

    protected override void OnUpdate(in float t)
    {
        // 遍历所有符合条件的实体，更新其速度组件
        var f = t;
        World.Query(in _query, 
            (ref InputDirection input, ref MoveSpeed move, ref Velocity vel) =>
            {
                // 当存在水平输入时，直接设置水平速度
                if (Math.Abs(input.X) > Epsilon)
                {
                    vel.X = input.X * move.Speed;
                }
                // 当无水平输入时，应用摩擦力逐渐减速至静止
                else
                {
                    vel.X = MoveToward(vel.X, 0, move.Friction * f);
                }
            });
    }

    /// <summary>
    ///     数值渐近函数，将数值从起始值逐步接近目标值
    /// </summary>
    /// <param name="from">起始数值</param>
    /// <param name="to">目标数值</param>
    /// <param name="delta">每次变化的最大幅度</param>
    /// <returns>调整后的数值，保证不会超过目标值</returns>
    private static float MoveToward(float from, float to, float delta)
    {
        return from < to ? Math.Min(from + delta, to) : Math.Max(from - delta, to);
    }
}
