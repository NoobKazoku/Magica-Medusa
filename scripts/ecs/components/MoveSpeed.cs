
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
using System.Runtime.InteropServices;

namespace MagicaMedusa.scripts.ecs.components;

/// <summary>
///     移动速度结构体，用于表示实体的移动速度和摩擦力属性
///     通常在ECS系统中作为组件使用，控制实体的移动行为
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MoveSpeed
{
    /// <summary>
    ///     移动速度值，表示实体每秒移动的距离单位
    /// </summary>
    public float Speed { get; set; }
    
    /// <summary>
    ///     摩擦力系数，用于计算速度衰减，值域通常在0-1之间
    ///     数值越小表示摩擦力越大，速度衰减越快
    /// </summary>
    public float Friction { get; set; }
}
