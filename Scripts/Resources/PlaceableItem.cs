
// PlaceableItem.cs
using Godot;

// Item을 상속받아 Item의 모든 속성(이름, 아이콘 등)을 그대로 사용합니다.
[GlobalClass]
public partial class PlaceableItem : Item
{
    // 이 아이템을 월드에 배치할 때, 실제로 소환될 씬(Scene) 파일을 여기에 연결합니다.
    [Export]
    public PackedScene PlacedScene { get; private set; }
}