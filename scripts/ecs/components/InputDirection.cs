
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
///     输入方向结构体，用于表示二维空间中的方向向量
///     包含X轴和Y轴的分量值，通常用于处理玩家输入或实体移动方向
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct InputDirection
{
    /// <summary>
    ///     X轴方向分量
    /// </summary>
    public float X { get; set; }
    
    /// <summary>
    ///     Y轴方向分量
    /// </summary>
    public float Y { get; set; }
}
