// AttackComponent.cs
using Godot;

public partial class AttackComponent : Node
{
    [Export]
    public float AttackDamage { get; private set; } = 10.0f;

    [Export]
    public float AttackRange { get; private set; } = 50.0f;

    [Export]
    public float AttackCooldown { get; private set; } = 1.0f;

    [Export]
    public float AttackWindup { get; private set; } = 0.3f;

    private Timer _cooldownTimer;
    private Timer _windupTimer; // 선딜레이를 위한 타이머를 추가합니다.
    private Node2D _currentTarget; // 현재 공격 중인 목표를 저장할 변수

    public override void _Ready()
    {
        // 쿨타임 타이머 설정
        _cooldownTimer = new Timer();
        _cooldownTimer.WaitTime = AttackCooldown;
        _cooldownTimer.OneShot = true;
        AddChild(_cooldownTimer);

        // 선딜레이 타이머 설정
        _windupTimer = new Timer();
        _windupTimer.WaitTime = AttackWindup;
        _windupTimer.OneShot = true;
        // 선딜레이 타이머가 끝나면, _OnWindupTimerTimeout 함수를 호출하도록 시그널을 연결합니다.
        _windupTimer.Timeout += _OnWindupTimerTimeout;
        AddChild(_windupTimer);
    }

    public bool CanAttack()
    {
        // 쿨타임과 선딜레이가 모두 진행 중이 아닐 때만 공격 가능합니다.
        return _cooldownTimer.IsStopped() && _windupTimer.IsStopped();
    }

    /// <summary>
    /// 공격을 '시작'하는 함수. 실제 데미지는 타이머 이후에 들어갑니다.
    /// </summary>
    public void PerformAttack(Node2D target)
    {
        if (!CanAttack())
        {
            return;
        }

        // 1. 공격을 시작했으므로, 즉시 쿨타임을 돌립니다.
        _cooldownTimer.Start();

        // 2. 현재 목표물을 저장하고, 선딜레이 타이머를 시작합니다.
        _currentTarget = target;
        _windupTimer.Start();

        // TODO: 이 시점에서 부모에게 공격 애니메이션을 재생하라고 알리는 것이 좋습니다.
        // GetParent<Zombie>().PlayAttackAnimation();
    }

    /// <summary>
    /// 선딜레이 타이머(_windupTimer)가 끝났을 때 자동으로 호출되는 함수.
    /// </summary>
    private void _OnWindupTimerTimeout()
    {
        // 선딜레이가 끝난 시점에, 저장해두었던 목표물이 여전히 유효한지 확인합니다.
        if (
            IsInstanceValid(_currentTarget)
            && _currentTarget.GetNodeOrNull<HealthComponent>("HealthComponent")
                is HealthComponent targetHealth
        )
        {
            GD.Print(
                $"{GetParent().Name}이(가) {_currentTarget.Name}을(를) {AttackDamage}의 피해로 공격!"
            );
            targetHealth.TakeDamage(AttackDamage);
        }
        else
        {
            GD.Print("공격하는 동안 목표물이 사라졌습니다.");
        }

        // 다음 공격을 위해 현재 목표물 정보를 초기화합니다.
        _currentTarget = null;
    }
}
