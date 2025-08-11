using Godot;

namespace projectgodot.Components
{
    public partial class PlayerAnimationController : Node
    {
        private AnimationTree _animationTree;

        private const string IDLE_BLEND_POSITION_PATH = "parameters/StateMachine/MoveState/IdleState/blend_position";
        private const string  RUN_BLEND_POSITION_PATH = "parameters/StateMachine/MoveState/RunState/blend_position";

        private Vector2 _lastDirection = new Vector2(0, -1);


        public void Initialize(AnimationTree animationTree)
        {
            _animationTree = animationTree;
            _animationTree.Active = true;
        }

        public void UpdateAnimation(Vector2 inputDirection)
        {
            if (_animationTree == null) 
            {
                GD.PrintErr("AnimationTree is null!");
                return;
            }

            // InputDirection의 길이로 움직임 감지 (0~1 범위)
            var inputLength = inputDirection.Length();
            var isMoving = inputLength > 0.1f;
            
            // playback 객체를 사용해서 직접 상태 전환 제어
            var movePlaybackVariant = _animationTree.Get("parameters/StateMachine/MoveState/playback");
            if (movePlaybackVariant.VariantType != Variant.Type.Nil)
            {
                var movePlayback = movePlaybackVariant.AsGodotObject() as AnimationNodeStateMachinePlayback;
                if (movePlayback != null)
                {
                    var currentState = movePlayback.GetCurrentNode();
                    
                    if (isMoving && currentState == "IdleState")
                    {
                        movePlayback.Travel("RunState");
                    }
                    else if (!isMoving && currentState == "RunState")
                    {
                        movePlayback.Travel("IdleState");
                    }
                }
            }

            // 블렌드용 방향 벡터 (Y축 반전)
            var directionVector = new Vector2(inputDirection.X, -inputDirection.Y);

            // 움직임 상태 변화 감지
            var wasMoving = _lastDirection.Length() > 0.1f;


            // 블렌드 포지션 설정
            if (isMoving)
            {
                _animationTree.Set(RUN_BLEND_POSITION_PATH, directionVector);
                _lastDirection = directionVector;
            }
            else
            {
                _animationTree.Set(IDLE_BLEND_POSITION_PATH, _lastDirection);
            }
        }
    }
}
