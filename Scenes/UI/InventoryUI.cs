// InventoryUI.cs
using Godot;
using Godot.Collections; // Dictionary를 사용하기 위해 필요

public partial class InventoryUI : PanelContainer
{
    // 슬롯 템플릿 씬을 에디터에서 미리 로드해 둡니다.
    [Export]
    private PackedScene _inventorySlotScene;

    private GridContainer _slotGrid;

    public override void _Ready()
    {
        _slotGrid = GetNode<GridContainer>("MarginContainer/ScrollContainer/SlotGrid");

        // 1. InventoryManager의 방송을 구독 신청합니다.
        InventoryManager.Instance.InventoryChanged += OnInventoryChanged;

        // 2. 초기에 빈 슬롯들을 미리 몇 개 만들어 둡니다 (선택 사항).
        // 또는 초기 인벤토리 상태를 기반으로 업데이트를 한 번 호출합니다.
        // OnInventoryChanged(InventoryManager.GetCurrentInventory()); // 이런 함수가 있다고 가정
    }

    // 인벤토리가 바뀔 때마다 호출될 핵심 함수
    private void OnInventoryChanged(Dictionary<Item, int> newInventory)
    {
        // 1. 기존의 모든 슬롯을 지웁니다.
        foreach (Node child in _slotGrid.GetChildren())
        {
            child.QueueFree();
        }

        // 2. 새로운 인벤토리 내용을 기반으로 슬롯을 다시 만듭니다.
        foreach (var entry in newInventory)
        {
            Item item = entry.Key;
            int quantity = entry.Value;

            // 슬롯 템플릿으로 새 인스턴스를 생성
            InventorySlot newSlot = _inventorySlotScene.Instantiate<InventorySlot>();
            // 그리드에 자식으로 추가합니다.
            _slotGrid.AddChild(newSlot);
            // 슬롯의 내용을 채웁니다.
            newSlot.UpdateSlot(item, quantity);
        }
    }
}
