using Godot;

// Tree.cs
public partial class Tree : StaticBody2D, IInteractable
{
    public void Interact(Node2D interactor)
    {
        GD.Print($"{interactor.Name}님이 나무를 채집했습니다!");
        // 여기에 목재를 생성하고, 나무를 파괴하는 로직을 추가.
        QueueFree();
    }
}