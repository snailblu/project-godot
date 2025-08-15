// PlayerAttackComponent.cs
using System;
using Godot;

public partial class PlayerAttackComponent : Node, IAttack, IDirectionalAttack // 인터페이스 구현
{
    [Export]
    public float AttackDamage { get; private set; } = 25.0f;

    [Export]
    public double AttackCoolDown { get; private set; } = 0.8;

    [Export]
    private Node2D _attackPivot;

    public event Action OnAttackPerformed;

    private Area2D _attackArea; // 실제 공격 판정 영역
    private Timer _cooldownTimer;
    private bool _canAttack = true;
    private bool _isInitialized = false; // 초기화 상태 추적

    public override void _Ready()
    {
        if (_attackPivot == null)
        {
            GD.PrintErr("PlayerAttackComponent에 AttackPivot이 연결되지 않았습니다! 컴포넌트가 비활성화됩니다.");
            return; // 초기화 실패 시 즉시 종료
        }

        // AttackArea 초기화 시도
        _attackArea = _attackPivot.GetNodeOrNull<Area2D>("AttackArea");
        if (_attackArea == null)
        {
            GD.PrintErr("PlayerAttackComponent: AttackPivot에 AttackArea가 없습니다! 컴포넌트가 비활성화됩니다.");
            return;
        }

        // 타이머 초기화
        _cooldownTimer = new Timer { WaitTime = AttackCoolDown, OneShot = true };
        AddChild(_cooldownTimer);
        _cooldownTimer.Timeout += () =>
        {
            _canAttack = true;
            GD.Print("PlayerAttackComponent: Attack cooldown finished, can attack again");
        };

        // 모든 초기화가 성공한 경우에만 활성화
        _isInitialized = true;
        GD.Print("PlayerAttackComponent: Initializing attack component");
        GD.Print(
            $"PlayerAttackComponent: Ready - AttackDamage={AttackDamage}, CoolDown={AttackCoolDown}"
        );
    }

    public bool CanAttack() => _isInitialized && _canAttack;

    // 인터페이스의 요구사항을 만족시킵니다. target 인자는 사용하지 않습니다.
    public void PerformAttack(Vector2 direction)
    {
        if (!CanAttack())
        {
            if (!_isInitialized)
                GD.Print("PlayerAttackComponent: Attack attempted but component not initialized");
            else
                GD.Print("PlayerAttackComponent: Attack attempted but on cooldown");
            return;
        }

        // 추가 안전성 검사
        if (_attackPivot == null || _attackArea == null)
        {
            GD.PrintErr("PlayerAttackComponent: Required nodes are null during attack");
            return;
        }

        GD.Print("PlayerAttackComponent: Starting attack sequence");
        _canAttack = false;
        _cooldownTimer.Start();

        // 공격 이벤트를 먼저 발행하여 애니메이션이 시작되도록 함
        OnAttackPerformed?.Invoke();

        _attackPivot.Rotation = direction.Angle();

        var overlappingBodies = _attackArea.GetOverlappingBodies();
        GD.Print($"PlayerAttackComponent: Found {overlappingBodies.Count} overlapping bodies");

        foreach (var body in overlappingBodies)
        {
            IDamageable damageable;
            
            // First try direct interface check
            if (body is IDamageable directDamageable)
            {
                damageable = directDamageable;
            }
            // If not directly implementing, look for component
            else
            {
                damageable = body.GetNodeOrNull<HealthComponent>("HealthComponent");
            }

            if (damageable != null)
            {
                GD.Print(
                    $"PlayerAttackComponent: Attacking {body.Name} with {AttackDamage} damage"
                );
                damageable.TakeDamage(AttackDamage);
            }
            else
            {
                GD.Print($"PlayerAttackComponent: {body.Name} is not damageable");
            }
        }
    }
}
