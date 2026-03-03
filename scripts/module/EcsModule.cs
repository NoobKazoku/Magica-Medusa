
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

using GFramework.Core.Abstractions.architecture;
using MagicaMedusa.scripts.ecs.features.movement;

namespace MagicaMedusa.scripts.module;

/// <summary>
///     ECS模块类，负责注册和管理ECS系统组件
///     实现IArchitectureModule接口，用于在游戏架构中安装ECS相关系统
/// </summary>
public class EcsModule: IArchitectureModule
{
    /// <summary>
    ///     安装方法，将ECS系统注册到游戏架构中
    /// </summary>
    /// <param name="architecture">游戏架构实例，用于注册系统组件</param>
    public void Install(IArchitecture architecture)
    {
        // 按执行顺序注册 ECS 系统
        // 1. 水平移动系统
        architecture.RegisterSystem(new MovementSystem());
        // 2. 重力系统
        architecture.RegisterSystem(new GravitySystem());
        // 3. 跳跃系统
        architecture.RegisterSystem(new JumpSystem());
        // 4. 物理同步系统（ECS 与 Godot 物理引擎的桥梁）
        architecture.RegisterSystem(new PhysicsSyncSystem());
        // 5. 面向系统
        architecture.RegisterSystem(new FacingSystem());
    }
}
