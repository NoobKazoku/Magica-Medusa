using Godot;
using System;

/// <summary>
///     血条组件：管理血量逻辑 + 视觉反馈
/// </summary>
[Tool] 
public partial class HealthBar : ProgressBar
{
    /// <summary>
    ///     死亡事件委托
    /// </summary>
    [Signal]
    public delegate void DeadEventHandler();

    // 以下导出部分作为测试,实际可去除
    //[ExportGroup("Test")]
    [Export] private float TestValue = 0f;
    [ExportToolButton("Heal")]
    public Callable ToHealTestButton => Callable.From(ToHeal);
    public void ToHeal() => Heal(TestValue);

    [ExportToolButton("Harm")]
    public Callable ToHarmTestButton => Callable.From(ToHarm);
    public void ToHarm() => Harm(TestValue);

    /// <summary>
    ///     设置最大血量并重置当前血量为满
    /// </summary>
    /// <param name="maxHealth">新的最大血量值（必须 > 0）</param>
    public void SetMaxHealth(float maxHealth)
    {
        if (maxHealth <= 0)
        {
            GD.PushError($"{nameof(SetMaxHealth)}: 最大血量必须大于 0");
            return;
        }
        MaxValue = maxHealth;
        Value = MaxValue; // 重置为满血
    }

    /// <summary>
    ///     恢复指定血量
    /// </summary>
    /// <param name="amount">恢复量</param>
    public void Heal(float amount)
    {
        if (amount <= 0 || float.IsNaN(amount) || float.IsInfinity(amount)) return;
        Value = Mathf.Min(Value + amount, MaxValue);
    }

    /// <summary>
    ///     扣除指定血量
    /// </summary>
    /// <param name="amount">伤害值</param>
    public void Harm(float amount)
    {
        if (amount <= 0 || Value <= 0 || float.IsNaN(amount) || float.IsInfinity(amount)) return;
        Value = Mathf.Max(Value - amount, 0);
        
        if (IsDead())
        {
            EmitSignal(SignalName.Dead);
            GD.Print($"{nameof(Harm)}: {nameof(IsDead)}");
        }
        
    }

    /// <summary>
    ///     检查实体是否已死亡
    /// </summary>
    /// <returns>已死亡时返回 true</returns>
    public bool IsDead() => Value <= 0;

}