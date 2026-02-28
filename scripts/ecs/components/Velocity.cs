
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
///     速度结构体，用于表示二维空间中实体的瞬时速度向量
///     包含X轴和Y轴的速度分量，通常用于物理计算和运动系统
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Velocity
{
    /// <summary>
    ///     X轴速度分量，单位为距离单位/秒
    /// </summary>
    public float X { get; set; }
    
    /// <summary>
    ///     Y轴速度分量，单位为距离单位/秒
    /// </summary>
    public float Y { get; set; }
}
