using System.Linq;
using Godot;

namespace projectgodot
{
    /// <summary>
    /// Component that handles smart interaction target selection based on distance and direction.
    /// Improves UX by prioritizing closest targets in the player's facing direction.
    /// </summary>
    public partial class InteractionComponent : Node2D
    {
        [Export]
        public float InteractionRadius { get; set; } = 100.0f;

        [Export]
        public float DirectionWeight { get; set; } = 0.5f; // How much to favor targets in facing direction

        private Area2D _interactionArea;
        private Node2D _owner;

        public override void _Ready()
        {
            _owner = GetParent<Node2D>();
            if (_owner == null)
            {
                GD.PrintErr("InteractionComponent must be a child of Node2D!");
                return;
            }

            _interactionArea = GetNode<Area2D>("InteractionArea");
            if (_interactionArea == null)
            {
                GD.PrintErr("InteractionComponent requires an InteractionArea child node!");
                return;
            }
        }

        /// <summary>
        /// Find the best interaction target based on distance and facing direction
        /// </summary>
        /// <param name="facingDirection">The direction the player is facing</param>
        /// <returns>The best IInteractable target, or null if none found</returns>
        public IInteractable GetBestInteractionTarget(Vector2 facingDirection)
        {
            if (_interactionArea == null || _owner == null)
                return null;

            var overlappingBodies = _interactionArea.GetOverlappingBodies();
            var interactables = overlappingBodies.OfType<IInteractable>().Cast<Node2D>().ToList();

            if (!interactables.Any())
                return null;

            // If only one target, return it
            if (interactables.Count == 1)
                return interactables[0] as IInteractable;

            // Calculate weighted scores for each target
            var bestTarget = interactables
                .Select(target => new
                {
                    Target = target,
                    Score = CalculateInteractionScore(target, facingDirection),
                })
                .OrderByDescending(x => x.Score)
                .First();

            return bestTarget.Target as IInteractable;
        }

        /// <summary>
        /// Calculate interaction score based on distance and direction
        /// Higher score = better target
        /// </summary>
        private float CalculateInteractionScore(Node2D target, Vector2 facingDirection)
        {
            // Distance component (closer = higher score)
            float distance = _owner.GlobalPosition.DistanceTo(target.GlobalPosition);
            float distanceScore = Mathf.Max(0, InteractionRadius - distance) / InteractionRadius;

            // Direction component (more aligned with facing = higher score)
            Vector2 toTarget = (target.GlobalPosition - _owner.GlobalPosition).Normalized();
            float directionDot = facingDirection.Dot(toTarget);
            float directionScore = (directionDot + 1) * 0.5f; // Convert from [-1,1] to [0,1]

            // Combine scores with weighting
            return distanceScore * (1 - DirectionWeight) + directionScore * DirectionWeight;
        }

        /// <summary>
        /// Try to interact with the best available target
        /// </summary>
        /// <param name="interactor">The entity performing the interaction</param>
        /// <param name="facingDirection">The direction the interactor is facing</param>
        /// <returns>True if interaction occurred, false otherwise</returns>
        public bool TryInteract(Node2D interactor, Vector2 facingDirection)
        {
            var target = GetBestInteractionTarget(facingDirection);
            if (target != null)
            {
                target.Interact(interactor);
                return true;
            }
            return false;
        }
    }
}
