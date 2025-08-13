using Godot;

// 이 어트리뷰트는 이 클래스가 Godot 에디터의 '새 리소스...' 메뉴에 나타나게 합니다.
[GlobalClass]
public partial class Item : Resource
{

    [Export] public string Name { get; private set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; private set; }
    [Export] public Texture2D Icon { get; private set; }

}
