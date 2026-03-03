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
///     跳跃状态组件，存储跳跃和重力相关的配置参数
/// </summary>
public struct JumpState
{
    /// <summary>
    ///     跳跃初速度（向上）
    /// </summary>
    public float JumpForce;

    /// <summary>
    ///     重力加速度
    /// </summary>
    public float Gravity;

    /// <summary>
    ///     最大下落速度
    /// </summary>
    public float MaxFallSpeed;

    /// <summary>
    ///     是否在地面上
    /// </summary>
    public bool IsGrounded;

    /// <summary>
    ///     离开地面后的缓冲帧数（土狼时间）
    /// </summary>
    public int CoyoteFrames;
}
