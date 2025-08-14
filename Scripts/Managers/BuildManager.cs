// BuildManager.cs
using Godot;

public partial class BuildManager : Node
{
    public static BuildManager Instance { get; private set; }
    private PlaceableItem _currentItemToBuild; // 현재 건설하려고 선택한 아이템
    private Node2D _placementPreview; // 마우스를 따라다니는 반투명 미리보기 노드
    private bool _canPlace = false;
    private bool _isEnteringBuildModeThisFrame = false;

    public override void _Ready()
    {
        Instance = this;
    }

    // 플레이어가 인벤토리 UI에서 건설할 아이템을 선택했을 때 호출됩니다.
    public void EnterBuildMode(PlaceableItem item)
    {
        // 이미 다른 것을 건설 중이었다면 이전 미리보기를 지웁니다.
        if (_placementPreview != null)
        {
            _placementPreview.QueueFree();
        }

        _currentItemToBuild = item;

        // 선택된 아이템의 씬을 기반으로 미리보기 인스턴스를 생성합니다.
        if (_currentItemToBuild != null && _currentItemToBuild.PlacedScene != null)
        {
            _placementPreview = (Node2D)_currentItemToBuild.PlacedScene.Instantiate();
            // 미리보기가 반투명하게 보이도록 설정합니다.
            _placementPreview.Modulate = new Color(1, 1, 1, 0.5f);
            GetTree().Root.AddChild(_placementPreview);
            _isEnteringBuildModeThisFrame = true;
        }
    }

    // 건설 모드를 빠져나갈 때 (예: ESC 키, 다른 아이템 선택)
    public void ExitBuildMode()
    {
        if (_placementPreview != null)
        {
            _placementPreview.QueueFree();
            _placementPreview = null;
        }
        _currentItemToBuild = null;
    }

    public override void _Process(double delta)
    {
        // 건설 모드가 아니면 아무것도 하지 않습니다.
        if (_currentItemToBuild == null || _placementPreview == null) return;
        if (_isEnteringBuildModeThisFrame)
        {
            // 플래그를 다시 false로 바꾸고, 이번 프레임의 모든 입력 처리를 건너뜁니다.
            _isEnteringBuildModeThisFrame = false;
            return;
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            ExitBuildMode();
            return; // 취소했으므로 아래 로직을 더 이상 실행할 필요가 없습니다.
        }

        // 1. 마우스 위치를 가져와 그리드에 맞춥니다.
        Vector2 mouseWorldPos = _placementPreview.GetGlobalMousePosition();
        Vector2 gridPos = SnapToGrid(mouseWorldPos);
        _placementPreview.GlobalPosition = gridPos;

        _canPlace = CheckPlacementValidity(gridPos);
        _placementPreview.Modulate = _canPlace ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);

        // 3. 마우스 왼쪽 클릭 시 건설을 시도합니다.
        if (Input.IsActionJustPressed("build_confirm"))
        {
            if (_canPlace)
            {
                PlaceStructure(gridPos);
            }
            else
            {
                // TODO: 여기에 "띵!" 하는 실패 효과음을 재생할 수 있습니다.
            }
        }
    }

    // 그리드 스냅 로직
    private Vector2 SnapToGrid(Vector2 position, int gridSize = 32)
    {
        float fGridSize = gridSize;
        float snappedX = Mathf.Round(position.X / fGridSize) * fGridSize;
        float snappedY = Mathf.Round(position.Y / fGridSize) * fGridSize;
        return new Vector2(snappedX, snappedY);
    }

    // 실제 구조물을 배치하는 함수
    private void PlaceStructure(Vector2 position)
    {
        // 1. 인벤토리에서 해당 아이템이 1개 이상 있는지 확인합니다.
        if (InventoryManager.Instance.GetItemCount(_currentItemToBuild) > 0)
        {
            // 2. 인벤토리에서 아이템 1개를 차감합니다.
            if (InventoryManager.Instance.RemoveItem(_currentItemToBuild, 1))
            {
                // 3. 새로운 인스턴스를 생성하여 월드에 배치합니다.
                Node2D newStructure = (Node2D)_currentItemToBuild.PlacedScene.Instantiate();
                newStructure.GlobalPosition = position;
                
                // 구조물이 배치될 부모 노드에 추가합니다. (예: 월드 씬의 특정 레이어)
                Node structuresNode = GetNode("/root/Game/WorldObjects");
                if (structuresNode != null)
                {
                    structuresNode.AddChild(newStructure);
                }
            }
        }
    }
    private bool CheckPlacementValidity(Vector2 position)
    {
        if (_placementPreview == null) return false;

        // 미리보기 노드에서 Area2D 검사기를 찾아옵니다.
        var validator = _placementPreview.GetNode<Area2D>("PlacementValidator");
        if (validator == null)
        {
            // 검사기가 없는 구조물은 어디에나 지을 수 있다고 가정합니다. (또는 항상 false 처리)
            return true;
        }

        // 현재 겹쳐있는 다른 Area나 Body가 있는지 확인합니다.
        var overlappingAreas = validator.GetOverlappingAreas();
        var overlappingBodies = validator.GetOverlappingBodies();

        // 겹치는 것이 하나라도 있으면, 배치가 불가능합니다.
        if (overlappingAreas.Count > 0 || overlappingBodies.Count > 0)
        {
            return false;
        }

        // TODO: 여기에 타일맵 지형 검사 로직을 추가할 수 있습니다.
        // var tileMap = GetNode<TileMap>("/root/Game/World/TileMap");
        // if (tileMap.IsOnInvalidTerrain(position)) return false;

        return true; // 모든 검사를 통과하면, 배치가 가능합니다.
    }
}