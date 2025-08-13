using Godot;

// IInteractable.cs (상호작용 계약서)
public interface IInteractable
{
    void Interact(Node2D interactor); // 누가 상호작용을 했는지 알려줌
}