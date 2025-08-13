using Godot;
using Godot.Collections; // Array를 사용하기 위해 필요

// 재료 하나를 표현하기 위한 작은 도우미 클래스
[GlobalClass]
public partial class Ingredient : Resource
{
    [Export] public Item Resource { get; private set; }
    [Export] public int Quantity { get; private set; }
}
