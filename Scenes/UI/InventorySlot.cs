// InventorySlot.cs
using Godot;

public partial class InventorySlot : PanelContainer
{
    private TextureRect _icon;
    private Label _quantityLabel;
    private Item _itemData; // 이 슬롯이 어떤 아이템을 대표하는지 저장

    public override void _Ready()
    {
        _icon = GetNode<TextureRect>("MarginContainer/VBoxContainer/Icon");
        _quantityLabel = GetNode<Label>("MarginContainer/VBoxContainer/QuantityLabel");
    }

    // 외부에서 이 슬롯의 내용을 채울 때 호출하는 함수
    public void UpdateSlot(Item item, int quantity)
    {
        _itemData = item;
        _icon.Texture = item.Icon;
        _quantityLabel.Text = $"x{quantity}";
        Visible = true; // 슬롯을 보이게 처리
    }

    // 슬롯이 비었을 때 호출하는 함수
    public void ClearSlot()
    {
        _itemData = null;
        _icon.Texture = null;
        _quantityLabel.Text = "";
        Visible = false; // 슬롯을 숨김 처리 (또는 기본 이미지 표시)
    }

    // 이 슬롯이 클릭되었을 때의 처리 (GUI 입력 이벤트)
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            // 이 슬롯이 PlaceableItem을 가지고 있다면, 건설 모드로 진입
            if (_itemData is PlaceableItem placeableItem)
            {
                // 전역 BuildManager에게 건설 모드 진입을 요청
                BuildManager.Instance.EnterBuildMode(placeableItem);
                GD.Print($"{_itemData.Name} 선택. 건설 모드로 진입합니다.");
                GetViewport().SetInputAsHandled();
            }
        }
    }
}