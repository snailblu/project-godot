using Godot;
using projectgodot.Scripts.Interfaces;

namespace projectgodot.Components
{
    public partial class PlayerAnimationController : Node, IPlayerAnimation
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

            var idle = inputDirection.Length() == 0;
            var directionVector = new Vector2(inputDirection.X, -inputDirection.Y);
            if (!idle)
            {
                _lastDirection = directionVector;
            }

            _animationTree.Set("parameters/StateMachine/MoveState/conditions/idle", idle);
            _animationTree.Set("parameters/StateMachine/MoveState/conditions/run", !idle);

            _animationTree.Set(RUN_BLEND_POSITION_PATH, _lastDirection);
            _animationTree.Set(IDLE_BLEND_POSITION_PATH, _lastDirection);
        }
    }
}
