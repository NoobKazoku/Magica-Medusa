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
///     面向方向结构体，用于表示实体的朝向状态
///     通常用于控制精灵渲染方向或确定攻击方向
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Facing
{
    /// <summary>
    ///     面向右侧属性，提供对面向状态的安全访问
    ///     true表示面向右方，false表示面向左方
    /// </summary>
    public bool FacingRight { get; set; }
}