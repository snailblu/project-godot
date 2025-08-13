// ResourceData.cs
using Godot;

// 이 어트리뷰트는 이 클래스가 Godot 에디터의 '새 리소스...' 메뉴에 나타나게 합니다.
[GlobalClass]
public partial class ResourceData : Resource
{
    [Export] public Texture2D Icon { get; set; } // 인벤토리 UI에 표시될 아이콘
    [Export] public int AmountPerHit { get; set; } = 1; // 한 번 채집 시 획득량
}
