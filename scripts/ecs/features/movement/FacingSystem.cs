
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
///     面向系统类，负责根据实体的速度自动更新其面向方向
///     通过监听Velocity和Facing组件，实现角色朝向与移动方向的同步
/// </summary>
public class FacingSystem : EcsSystemBase
{
    /// <summary>
    ///     ECS查询描述符，用于筛选同时具有速度和面向组件的实体
    /// </summary>
    private QueryDescription _query;

    /// <summary>
    ///     ECS系统初始化方法，在系统启动时配置查询条件
    ///     设置查询需要同时包含Velocity和Facing两个组件的实体
    /// </summary>
    protected override void OnEcsInit()
    {
        _query = new QueryDescription()
            .WithAll<Velocity, Facing>();
    }

    /// <summary>
    ///     系统更新方法，每帧检查并更新实体的面向方向
    /// </summary>
    /// <param name="deltaTime">帧间隔时间，单位为秒</param>
    public override void Update(float deltaTime)
    {
        // 遍历所有具有速度和面向组件的实体
        World.Query(in _query, (ref Velocity vel, ref Facing facing) =>
        {
            // 根据水平速度分量确定面向方向
            // 正速度面向右侧，负速度面向左侧，零速度保持当前方向
            facing.FacingRight = vel.X switch
            {
                > 0 => true,
                < 0 => false,
                _ => facing.FacingRight,
            };
        });
    }
}
