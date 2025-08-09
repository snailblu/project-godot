using Godot;
using projectgodot.Constants;

namespace projectgodot
{
    public partial class Zombie : CharacterBody2D
    {
        [Export] public float MoveSpeed { get; set; } = GameConstants.Zombie.DEFAULT_MOVE_SPEED;
        [Export] public int InitialHealth { get; set; } = GameConstants.Zombie.DEFAULT_INITIAL_HEALTH;
        
        private Player _player;

        // 컴포넌트들을 '부품'처럼 가짐
        public HealthComponent Health { get; private set; }
        private ZombieAIComponent _ai;
        
        // 애니메이션 관리
        private AnimatedSprite2D _animatedSprite;
        private bool _isDying = false;

        public override void _Ready()
        {
            // Export된 InitialHealth 값으로 체력 컴포넌트 초기화
            Health = new HealthComponent(InitialHealth);
            Health.Died += OnDied;
            Health.HealthChanged += OnHealthChanged;

            // AI 컴포넌트 초기화
            _ai = new ZombieAIComponent();
            
            // 애니메이션 노드 참조 및 시작
            _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            _animatedSprite.Play("walk");

            // 플레이어 찾기 (더 좋은 방법은 나중에 개선 예정)
            CallDeferred(nameof(FindPlayer));
        }

        private void FindPlayer()
        {
            // Main 씬의 Player 노드를 찾습니다
            var mainScene = GetTree().CurrentScene;
            if (mainScene != null)
            {
                _player = mainScene.GetNodeOrNull<Player>("Player");
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_player == null || _isDying) return;
            
            if (Health.IsDead) 
            {
                // 사망 처리는 OnDied()에서 처리
                return;
            }

            // 1. AI 컴포넌트로 방향 계산
            var direction = _ai.CalculateDirection(GlobalPosition, _player.GlobalPosition);

            // 2. 좌우 방향에 따라 스프라이트 플립
            if (direction.X < 0)
            {
                _animatedSprite.FlipH = true;  // 왼쪽으로 이동 (플립)
            }
            else if (direction.X > 0)
            {
                _animatedSprite.FlipH = false; // 오른쪽으로 이동 (기본 방향)
            }
            // direction.X == 0인 경우 현재 방향 유지

            // 3. 이동
            Velocity = direction * MoveSpeed * (float)delta;
            MoveAndSlide();
        }

        private async void OnDied()
        {
            if (_isDying) return; // 중복 처리 방지
            _isDying = true;
            
            var events = GetNode<Events>("/root/Events");
            
            // 충돌 비활성화 (총알이 더 이상 맞지 않게)
            SetCollisionLayerValue(2, false);
            
            // 사망 애니메이션 재생
            _animatedSprite.Play("die");
            
            // 기본 점수 이벤트 발생 (100점)
            events.EmitSignal(Events.SignalName.ZombieDied, 100);
            
            // 강화된 사망 효과 이벤트 발생 (좀비 타입과 위치 전달)
            events.EmitSignal(Events.SignalName.ZombieDeathEffectRequested, (int)ZombieType.Basic, GlobalPosition);
            
            // 화면 플래시 효과 요청
            events.EmitSignal(Events.SignalName.ScreenFlashRequested);
            
            // die 애니메이션이 끝날 때까지 대기
            await ToSignal(_animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);
            
            // 애니메이션 완료 후 추가로 1초 대기
            await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
            QueueFree();
        }

        private async void OnHealthChanged(int currentHealth)
        {
            // 죽어가는 중이면 hit 애니메이션 스킵
            if (_isDying) return;
            
            // hit 애니메이션 재생
            _animatedSprite.Play("hit");
            
            // 좀비가 데미지를 받았을 때 사운드 이벤트 발생
            var events = GetNode<Events>("/root/Events");
            events.EmitSignal(Events.SignalName.ZombieTookDamage);
            
            // 0.1초 후 walk 애니메이션으로 복귀
            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
            if (!_isDying) // 사망 중이 아니면 walk로 복귀
            {
                _animatedSprite.Play("walk");
            }
        }
    }
}