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

namespace MagicaMedusa.scripts.ecs.components;

/// <summary>
///     跳跃输入组件，存储玩家的跳跃输入状态
/// </summary>
public struct JumpInput
{
    /// <summary>
    ///     本帧是否按下跳跃键
    /// </summary>
    public bool JumpPressed;

    /// <summary>
    ///     跳跃键是否持续按住
    /// </summary>
    public bool JumpHeld;
}
