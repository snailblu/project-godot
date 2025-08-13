// LightingController.cs
using Godot;

public partial class LightingController : WorldEnvironment
{
    // 디자이너가 에디터에서 낮/밤 조명 환경을 미리 설정해 둘 수 있습니다.
    [Export] private Godot.Environment _dayEnvironment;
    [Export] private Godot.Environment _nightEnvironment;

    private TimeManager _timeManager;

    public override void _Ready()
    {
        // 전역 TimeManager에 접근
        _timeManager = GetNode<TimeManager>("/root/TimeManager");
        
        // TimeManager의 신호 구독
        _timeManager.DayStarted += OnDayStarted;
        _timeManager.NightStarted += OnNightStarted;

        // 게임 시작 시 초기 상태 설정
        if (_timeManager.IsDay)
        {
            OnDayStarted();
        }
        else
        {
            OnNightStarted();
        }
    }

    private void OnDayStarted()
    {
        this.Environment = _dayEnvironment;
        GD.Print("조명을 낮으로 변경합니다.");
    }

    private void OnNightStarted()
    {
        this.Environment = _nightEnvironment;
        GD.Print("조명을 밤으로 변경합니다.");
    }
}