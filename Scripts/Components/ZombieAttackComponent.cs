// AttackComponent.cs
using System;
using Godot;

public partial class ZombieAttackComponent : Node, IAttack, ITargetedAttack
{
    [Export]
    public float AttackDamage { get; private set; } = 10.0f;

    [Export]
    public float AttackRange { get; private set; } = 50.0f;

    [Export]
    public float AttackCooldown { get; private set; } = 1.0f;

    [Export]
    public float AttackWindup { get; private set; } = 0.3f;

    // 공격 생명주기 이벤트
    public event Action AttackStarted;
    public event Action AttackEnded;

    private Timer _cooldownTimer;
    private Timer _windupTimer; // 선딜레이를 위한 타이머를 추가합니다.
    private Node2D _currentTarget; // 현재 공격 중인 목표를 저장할 변수

    public override void _Ready()
    {
        // 쿨타임 타이머 설정
        _cooldownTimer = new Timer();
        _cooldownTimer.WaitTime = AttackCooldown;
        _cooldownTimer.OneShot = true;
        _cooldownTimer.Timeout += _OnCooldownTimerTimeout;
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
    public void PerformAttack(Node2D target = null)
    {
        if (!CanAttack() || target == null)
            return;

        _cooldownTimer.Start();
        _currentTarget = target;
        _windupTimer.Start();

        // 공격 시작을 알림
        AttackStarted?.Invoke();
    }

    /// <summary>
    /// 선딜레이 타이머(_windupTimer)가 끝났을 때 자동으로 호출되는 함수.
    /// </summary>
    private void _OnWindupTimerTimeout()
    {
        // 선딜레이가 끝난 시점에, 저장해두었던 목표물이 여전히 유효한지 확인합니다.
        IDamageable targetDamageable = null;
        
        if (IsInstanceValid(_currentTarget))
        {
            // First try direct interface check
            if (_currentTarget is IDamageable directDamageable)
            {
                targetDamageable = directDamageable;
            }
            // If not directly implementing, look for component
            else
            {
                targetDamageable = _currentTarget.GetNodeOrNull<HealthComponent>("HealthComponent") as IDamageable;
            }
        }

        if (targetDamageable != null)
        {
            GD.Print(
                $"{GetParent().Name}이(가) {_currentTarget.Name}을(를) {AttackDamage}의 피해로 공격!"
            );
            targetDamageable.TakeDamage(AttackDamage);
        }
        else
        {
            GD.Print("공격하는 동안 목표물이 사라졌거나 데미지를 받을 수 없습니다.");
        }

        // 다음 공격을 위해 현재 목표물 정보를 초기화합니다.
        _currentTarget = null;
    }

    /// <summary>
    /// 쿨타임 타이머가 끝났을 때 자동으로 호출되는 함수.
    /// </summary>
    private void _OnCooldownTimerTimeout()
    {
        // 공격이 완전히 끝났음을 알림
        AttackEnded?.Invoke();
    }
}
