// ResourceNode.cs
using Godot;

public partial class ResourceNode : StaticBody2D, IInteractable
{
    // 디자이너가 이 노드에 'WoodData.tres' 같은 데이터 파일을 드래그 앤 드롭으로 연결합니다.
    [Export]
    private Item _item;

    [Export]
    private int _quantity = 5;

    private HealthComponent _healthComponent;

    public override void _Ready()
    {
        // 내구도 역할을 할 HealthComponent를 찾아옵니다.
        _healthComponent = GetNode<HealthComponent>("HealthComponent");
        // 이 컴포넌트가 파괴되면(내구도가 0이 되면) 노드 전체를 제거하도록 연결합니다.
        _healthComponent.Died += OnDepleted;
    }

    // PlayerController가 호출할 상호작용 함수
    public void Interact(Node2D interactor)
    {
        // 인터랙션 한 주체가 Player인지 확인 (나중을 위해)
        if (interactor is PlayerController player)
        {
            GD.Print($"{player.Name}님이 {_item.ResourceName}을(를) 채집 시도.");
            
            // 플레이어의 인벤토리에 자원을 추가하는 로직 (지금은 임시로 출력)
            // 예: player.Inventory.AddItem(_resourceData, _resourceData.AmountPerHit);
            InventoryManager.Instance.AddItem(_item, _quantity);
            GD.Print($"{_quantity}개의 {_item.Name}을(를) 획득했습니다!");

            // 한 번 채집할 때마다 내구도를 10씩 깎는다 (임의의 값).
            _healthComponent.TakeDamage(10);
        }
    }

    // HealthComponent가 파괴(사망) 신호를 보내면 실행될 함수
    private void OnDepleted()
    {
        GD.Print($"{_item.ResourceName} 자원이 고갈되었습니다.");
        // 여기에 파괴 효과(파티클 등)를 추가할 수 있습니다.
        QueueFree(); // 자기 자신을 월드에서 제거
    }
}