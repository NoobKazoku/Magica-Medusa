
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
namespace MagicaMedusa.scripts.dialogue;

/// <summary>
///     对话序列，包含一组连续的对话
/// </summary>
public class DialogueSequence
{
    /// <summary>
    ///     对话列表
    /// </summary>
    public IList<DialogueData> Dialogues { get; init; } = [];

    /// <summary>
    ///     是否在对话期间暂停游戏，默认true
    /// </summary>
    public bool PauseGameDuringDialogue { get; set; } = true;

    /// <summary>
    ///     完成回调ID（可选）
    /// </summary>
    public string? OnCompleteCallbackId { get; set; }
}
