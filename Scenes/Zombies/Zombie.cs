// Zombie.cs
using Godot;

public partial class Zombie : CharacterBody2D
{
    // --- 상태 정의 ---
    private enum ZombieState
    {
        Idle,
        Chasing,
        Attacking,
        Dying,
    }

    private ZombieState _currentState = ZombieState.Idle;
    private ZombieState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnStateChanged(_currentState); // 상태가 변경될 때마다 이 함수 호출
            }
        }
    }

    // --- 컴포넌트 및 노드 참조 ---
    private HealthComponent _healthComponent;
    private AttackComponent _attackComponent;
    private Area2D _detectionArea;
    private Node2D _target = null; // 추적할 목표 (플레이어 또는 바리케이드)
    private AnimatedSprite2D _animatedSprite; // AnimatedSprite2D 참조 추가

    // --- 좀비 능력치 ---
    [Export]
    public float Speed { get; private set; } = 100.0f;

    public override void _Ready()
    {
        GD.Print($"[Zombie] _Ready() 시작 - 위치: {GlobalPosition}");

        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        GD.Print($"[Zombie] HealthComponent 획득: {_healthComponent != null}");

        _attackComponent = GetNode<AttackComponent>("AttackComponent");
        GD.Print($"[Zombie] AttackComponent 획득: {_attackComponent != null}");

        _detectionArea = GetNode<Area2D>("DetectionArea");
        GD.Print($"[Zombie] DetectionArea 획득: {_detectionArea != null}");

        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // 좀비가 죽으면, 자기 자신을 파괴하도록 시그널 연결
        _healthComponent.Died += OnDied;
        GD.Print("[Zombie] Died 시그널 연결 완료");

        // 감지 영역에 누군가 들어오거나 나갈 때, 목표물을 설정/해제
        _detectionArea.BodyEntered += OnDetectionAreaEntered;
        _detectionArea.BodyExited += OnDetectionAreaExited;
        GD.Print("[Zombie] DetectionArea 시그널 연결 완료");
        _animatedSprite.AnimationFinished += OnAnimationFinished;

        GD.Print($"[Zombie] _Ready() 완료 - 초기 상태: {_currentState}");
        CurrentState = ZombieState.Idle;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (CurrentState == ZombieState.Dying)
            return;

        // 목표물이 없다면 무조건 Idle 상태로 전환
        if (_target == null)
        {
            CurrentState = ZombieState.Idle;
        }

        switch (CurrentState)
        {
            case ZombieState.Idle:
                // Idle 상태에서는 할 일이 없지만, 혹시 목표물이 생겼는지 계속 확인할 수 있습니다.
                // (DetectionArea 시그널이 이 역할을 대신 해주므로 사실상 비워둬도 됨)
                break;
            case ZombieState.Chasing:
                ChaseTarget(delta);
                break;
            case ZombieState.Attacking:
                // Attacking 상태에서는 새로운 행동을 시작하는 대신,
                // 공격이 끝나고 다시 Chasing 상태로 돌아갈 조건만 체크합니다.
                CheckReturnToChasing();
                break;
        }
        UpdateSpriteDirection();
    }

    private void UpdateSpriteDirection()
    {
        // Velocity.X가 아주 작은 값일 때는 방향을 바꾸지 않도록 임계값을 줍니다.
        if (Mathf.Abs(Velocity.X) > 0.1f)
        {
            // Velocity.X가 양수(오른쪽)이면 FlipH = false (원본 방향)
            // Velocity.X가 음수(왼쪽)이면 FlipH = true (뒤집힌 방향)
            _animatedSprite.FlipH = Velocity.X < 0;
        }
    }

    // --- 상태가 변경될 때 호출되는 핵심 함수 ---
    private void OnStateChanged(ZombieState newState)
    {
        switch (newState)
        {
            case ZombieState.Idle:
                _animatedSprite.Play("idle");
                break;
            case ZombieState.Chasing:
                _animatedSprite.Play("walk");
                break;
            case ZombieState.Attacking:
                // 공격 애니메이션은 한 번만 재생되어야 하므로, 루프(Loop)를 꺼줍니다.
                _animatedSprite.Play("attack");
                break;
            case ZombieState.Dying:
                _animatedSprite.Play("die");
                break;
        }
    }

    private void OnDied()
    {
        GD.Print($"[Zombie] 사망 - 위치: {GlobalPosition}");
        // 즉시 QueueFree()를 호출하는 대신, 'Dying' 상태로 전환합니다.
        CurrentState = ZombieState.Dying;
        // 물리적 충돌을 비활성화하여 죽는 동안 다른 개체를 막지 않도록 합니다.
        SetPhysicsProcess(false);
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
    }

    private void OnAnimationFinished()
    {
        string finishedAnimation = _animatedSprite.Animation;

        // 1. 만약 'attack' 애니메이션이 끝났다면
        if (finishedAnimation == "attack")
        {
            // 아직 공격 쿨타임 중이므로, 대기(idle) 애니메이션을 재생합니다.
            // 상태는 여전히 Attacking이므로, 다음 프레임에 CheckReturnToChasing()이 호출됩니다.
            _animatedSprite.Play("idle");
        }
        // 2. 만약 'die' 애니메이션이 끝났다면
        else if (finishedAnimation == "die")
        {
            // 자기 자신을 파괴합니다.
            QueueFree();
        }
    }

    private void OnDeathAnimationFinished()
    {
        // animation_finished 시그널은 모든 애니메이션이 끝날 때마다 발생하므로,
        // 현재 애니메이션이 'die'일 때만 파괴 로직을 실행하도록 조건을 겁니다.
        if (_animatedSprite.Animation == "die")
        {
            QueueFree();
        }
    }

    private void OnDetectionAreaEntered(Node2D body)
    {
        GD.Print($"[Zombie] 감지 영역 진입: {body.Name}, 그룹: {body.GetGroups()}");

        // 감지된 대상이 'Player' 그룹에 속해 있다면
        if (body.IsInGroup("Player"))
        {
            _target = body;
            CurrentState = ZombieState.Chasing;
            GD.Print(
                $"[Zombie] 플레이어 발견! 추적 시작! 거리: {GlobalPosition.DistanceTo(body.GlobalPosition)}"
            );
        }
        else
        {
            GD.Print($"[Zombie] 플레이어가 아닌 객체 감지: {body.Name}");
        }
    }

    private void OnDetectionAreaExited(Node2D body)
    {
        GD.Print($"[Zombie] 감지 영역 이탈: {body.Name}, 현재 타겟과 동일: {body == _target}");

        // 목표물이 감지 영역을 벗어났다면
        if (body == _target)
        {
            _target = null;
            CurrentState = ZombieState.Idle;
            GD.Print("[Zombie] 목표물 상실. 대기 상태로 전환.");
        }
    }

    private void ChaseTarget(double delta)
    {
        float distanceToTarget = this.GlobalPosition.DistanceTo(_target.GlobalPosition);

        // --- 여기가 핵심적인 변경점 1 ---
        // 1. 공격이 가능한 '순간'인지 확인합니다. (거리 + 쿨타임)
        if (distanceToTarget <= _attackComponent.AttackRange && _attackComponent.CanAttack())
        {
            // 2. 공격을 '시작'하고 즉시 'Attacking' 상태로 전환합니다.
            _attackComponent.PerformAttack(_target);
            CurrentState = ZombieState.Attacking;
            Velocity = Vector2.Zero; // 공격 시작 시 움직임 멈춤
        }
        else
        {
            // 공격할 수 없는 상황이라면 계속 추적합니다.
            Vector2 direction = (_target.GlobalPosition - this.GlobalPosition).Normalized();
            Velocity = direction * Speed;
        }

        MoveAndSlide();
    }

    private void AttackTarget(double delta)
    {
        if (_target == null)
        {
            GD.Print("[Zombie] AttackTarget: 타겟이 null, Idle 상태로 변경");
            _currentState = ZombieState.Idle;
            return;
        }

        // 1. 목표물이 공격 범위를 벗어났는지 확인
        float distanceToTarget = this.GlobalPosition.DistanceTo(_target.GlobalPosition);
        if (distanceToTarget > _attackComponent.AttackRange)
        {
            GD.Print(
                $"[Zombie] AttackTarget: 타겟이 공격 범위를 벗어남, 거리: {distanceToTarget}, 범위: {_attackComponent.AttackRange}"
            );
            _currentState = ZombieState.Chasing;
            return;
        }

        // 2. AttackComponent를 통해 공격 실행
        // AttackComponent는 내부에 공격 쿨타임을 관리하는 로직을 가짐
        GD.Print($"[Zombie] AttackTarget: 공격 시도, 타겟: {_target.Name}");
        _attackComponent.PerformAttack(_target);
    }

    private void CheckReturnToChasing()
    {
        // 공격 애니메이션이나 쿨타임이 끝났다고 판단되면, 다시 추적 상태로 돌아갑니다.
        // 가장 간단한 방법은 '쿨타임이 다시 돌았는가'를 기준으로 삼는 것입니다.
        // 또는 공격 애니메이션의 길이를 기준으로 타이머를 사용할 수도 있습니다.
        // 여기서는 AttackComponent의 쿨타임이 끝나는 시점을 기준으로 합니다.
        if (_attackComponent.CanAttack())
        {
            CurrentState = ZombieState.Chasing;
        }

        // 또는, 목표물이 범위를 벗어났을 때도 즉시 추적 상태로 돌아가야 합니다.
        float distanceToTarget = this.GlobalPosition.DistanceTo(_target.GlobalPosition);
        if (distanceToTarget > _attackComponent.AttackRange)
        {
            CurrentState = ZombieState.Chasing;
        }
    }
}
