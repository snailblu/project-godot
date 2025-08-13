using Godot;
using Godot.Collections;
using System.IO;

// CraftingManager.cs (핵심 로직 의사코드)
public partial class CraftingManager : Node
{
    private Array<CraftingRecipe> _allRecipes = new Array<CraftingRecipe>();

    public override void _Ready()
    {
        // 프로젝트의 "res://recipes/" 폴더에 있는 모든 .tres 파일을 읽어와 _allRecipes에 저장
        LoadAllRecipesFromDirectory("res://Resources/Recipies/");

        // 로드가 잘 되었는지 확인하기 위한 테스트 출력
        GD.Print($"총 {_allRecipes.Count}개의 레시피를 로드했습니다.");

    }

    /// <summary>
    /// 지정된 디렉터리 경로에서 모든 CraftingRecipe 파일을 로드하여 _allRecipes 목록에 추가합니다.
    /// </summary>
    /// <param name="path">"res://"로 시작하는 프로젝트 디렉터리 경로</param>
    private void LoadAllRecipesFromDirectory(string path)
    {
        // 1. DirAccess를 사용하여 디렉터리에 접근합니다.
        using var dir = DirAccess.Open(path);

        if (dir != null)
        {
            // 2. 디렉터리의 모든 내용을 순회하기 시작합니다.
            dir.ListDirBegin();
            string fileName = dir.GetNext();

            while (fileName != "")
            {
                // 3. 현재 항목이 디렉터리가 아니고, .tres 파일로 끝나는지 확인합니다.
                // (우리의 레시피는 .tres 파일로 저장되기 때문입니다)
                if (!dir.CurrentIsDir() && fileName.EndsWith(".tres"))
                {
                    // 4. ResourceLoader를 사용하여 파일을 리소스로 로드합니다.
                    // 경로를 조합해야 합니다: "res://recipes/" + "Recipe_WoodenBarricade.tres"
                    string fullPath = Path.Combine(path, fileName);
                    var loadedResource = ResourceLoader.Load(fullPath);

                    // 5. 로드된 리소스가 우리가 원하는 CraftingRecipe 타입인지 확인합니다.
                    if (loadedResource is CraftingRecipe recipe)
                    {
                        _allRecipes.Add(recipe);
                    }
                }
                fileName = dir.GetNext();
            }
        }
        else
        {
            GD.PrintErr($"레시피 폴더를 열 수 없습니다: {path}");
        }
    }

    // 이 레시피를 제작할 수 있는가?
    public bool CanCraft(CraftingRecipe recipe)
    {
        // 레시피의 모든 재료에 대해 반복
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            // 만약 플레이어 인벤토리가 해당 재료를 필요량만큼 가지고 있지 않다면,
            if (InventoryManager.Instance.GetItemCount(ingredient.Resource) < ingredient.Quantity)
            {
                return false; // 제작 불가
            }
        }
        return true; // 모든 재료가 충분함 -> 제작 가능
    }

    // 이 레시피를 제작하라!
    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe)) return; // 제작 불가 시 즉시 중단

        // 1. 재료 차감
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            InventoryManager.Instance.RemoveItem(ingredient.Resource, ingredient.Quantity);
        }

        // 2. 결과물 추가
        InventoryManager.Instance.AddItem(recipe.Output.Resource, recipe.Output.Quantity);

        GD.Print($"{recipe.Output.Resource.Name} 제작 성공!");
    }
}