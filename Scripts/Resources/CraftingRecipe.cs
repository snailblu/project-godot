using Godot;
using Godot.Collections; // Array를 사용하기 위해 필요

[GlobalClass]
public partial class CraftingRecipe : Resource
{
    // 재료 목록: 어떤 'item'가 몇 개 필요한지 정의
    [Export]
    public Array<Ingredient> Ingredients { get; private set; }

    // 결과물: 어떤 'Item'가 몇 개 만들어지는지 정의
    [Export]
    public Ingredient Output { get; private set; }
}

