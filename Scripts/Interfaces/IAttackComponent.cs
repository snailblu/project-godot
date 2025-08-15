// IAttackComponent.cs
using Godot;

/// <summary>
/// 공격 기능을 수행하는 모든 컴포넌트가 구현해야 하는 인터페이스입니다.
/// </summary>
// IAttack.cs
public interface IAttack
{
    bool CanAttack();
}

// ITargetedAttack.cs
public interface ITargetedAttack : IAttack
{
    void PerformAttack(Node2D target);
}

// IDirectionalAttack.cs
public interface IDirectionalAttack : IAttack
{
    void PerformAttack(Vector2 direction);
}
