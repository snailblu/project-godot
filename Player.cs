using Godot;

namespace projectgodot
{
    public partial class Player : CharacterBody2D
    {
        private PlayerData _playerData;
        private PlayerMovement _playerMovement;

        public override void _Ready()
        {
            // TDD로 검증된 클래스들을 인스턴스화 (컴포지션 패턴)
            _playerData = new PlayerData();
            _playerMovement = new PlayerMovement();
        }

        public override void _PhysicsProcess(double delta)
        {
            // 1. 입력 받기
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

            // 2. TDD로 검증된 로직 클래스를 이용해 속도 계산
            Vector2 calculatedVelocity = _playerMovement.CalculateVelocity(
                direction, 
                _playerData.MovementSpeed, 
                (float)delta
            );

            // 3. Godot 노드에 결과 적용
            Velocity = calculatedVelocity;
            MoveAndSlide();
        }
    }
}