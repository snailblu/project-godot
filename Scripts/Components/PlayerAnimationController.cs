using Godot;

namespace projectgodot.Components
{
    public partial class PlayerAnimationController : Node
    {
        private AnimatedSprite2D _animatedSprite;

        public override void _Ready()
        {
            // 부모 Player 노드에서 AnimatedSprite2D 찾기
            _animatedSprite = GetParent().GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }

        public void UpdateAnimation(Vector2 velocity)
        {
            if (_animatedSprite == null) return;

            // 이동 애니메이션 처리
            if (velocity.Length() > 0)
            {
                // 속도가 있으면 'walk' 애니메이션 재생
                // 현재 애니메이션이 이미 'walk'가 아닐 때만 변경하여 불필요한 재시작 방지
                if (_animatedSprite.Animation != "walk")
                {
                    _animatedSprite.Play("walk");
                }
            }
            else
            {
                // 속도가 0이면 'idle' 애니메이션 재생
                if (_animatedSprite.Animation != "idle")
                {
                    _animatedSprite.Play("idle");
                }
            }

            // 이동 방향에 따라 좌우 반전
            if (velocity.X != 0)
            {
                _animatedSprite.FlipH = velocity.X < 0;
            }
        }
    }
}