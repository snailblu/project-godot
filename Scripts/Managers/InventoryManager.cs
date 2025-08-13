// InventoryManager.cs
using Godot;
using Godot.Collections; // Dictionary를 사용하기 위해 필요

public partial class InventoryManager : Node
{
    public static InventoryManager Instance { get; private set; }

    // --- 시그널 정의 ---
    [Signal]
    public delegate void InventoryChangedEventHandler(Dictionary<Item, int> newInventory);

    // --- 핵심 데이터 ---
    private Dictionary<Item, int> _inventory = new Dictionary<Item, int>();

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// 지정된 아이템을 인벤토리에 추가합니다.
    /// </summary>
    /// <param name="item">추가할 아이템의 Item</param>
    /// <param name="quantity">추가할 수량</param>
    public void AddItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return;

        // 인벤토리에 이미 해당 아이템이 있는지 확인
        if (_inventory.ContainsKey(item))
        {
            _inventory[item] += quantity;
        }
        else
        {
            _inventory[item] = quantity;
        }

        GD.Print($"{item.Name} {quantity}개 추가됨. 현재: {_inventory[item]}개");
        EmitSignal(SignalName.InventoryChanged, _inventory);
    }

    /// <summary>
    /// 지정된 아이템을 인벤토리에서 제거합니다.
    /// </summary>
    /// <param name="item">제거할 아이템의 ResourceData</param>
    /// <param name="quantity">제거할 수량</param>
    /// <returns>제거에 성공하면 true, 수량이 부족하면 false를 반환합니다.</returns>
    public bool RemoveItem(Item item, int quantity)
    {
        if (item == null || quantity <= 0) return false;

        // 제거할 아이템이 없거나, 수량이 부족하면 실패 처리
        if (!_inventory.ContainsKey(item) || _inventory[item] < quantity)
        {
            GD.PrintErr($"{item.Name} 제거 실패. (요청: {quantity}, 보유: {GetItemCount(item)})");
            return false;
        }

        _inventory[item] -= quantity;
        GD.Print($"{item.Name} {quantity}개 제거됨. 현재: {_inventory[item]}개");

        // 만약 아이템의 수량이 0이 되면, 딕셔너리에서 키 자체를 제거하여 깔끔하게 관리합니다.
        if (_inventory[item] == 0)
        {
            _inventory.Remove(item);
        }
        
        EmitSignal(SignalName.InventoryChanged, _inventory);
        return true;
    }

    /// <summary>
    /// 특정 아이템의 현재 보유 수량을 확인합니다.
    /// </summary>
    /// <param name="item">확인할 아이템의 ResourceData</param>
    /// <returns>보유 수량. 아이템이 없으면 0을 반환합니다.</returns>
    public int GetItemCount(Item item)
    {
        if (item == null) return 0;
        
        if (_inventory.ContainsKey(item))
        {
            // 2. 키가 존재할 때만 값을 반환합니다.
            return _inventory[item];
        }
        else
        {
            // 3. 키가 존재하지 않으면 기본값 0을 반환합니다.
            return 0;
        }
    }
}