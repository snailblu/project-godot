// TimeManager.cs
using Godot;

public partial class TimeManager : Node
{
    // --- 디자이너 설정 값 ---
    [Export] public float DayDurationSeconds { get; set; } = 6f; // 낮의 지속 시간 (초)
    [Export] public float NightDurationSeconds { get; set; } = 4.5f; // 밤의 지속 시간 (초)

    // --- 상태 변수 ---
    private float _timer; // 현재 사이클의 시간을 재는 타이머
    private bool _isDay = true; // 현재 낮인가?

    // --- 외부 공개 속성 (읽기 전용) ---
    public bool IsDay => _isDay;
    public float TimeRemaining => _isDay ? DayDurationSeconds - _timer : NightDurationSeconds - _timer;

    // --- 시그널 (방송 채널) ---
    [Signal] public delegate void DayStartedEventHandler();
    [Signal] public delegate void NightStartedEventHandler();

    public override void _Process(double delta)
    {
        _timer += (float)delta;

        // 현재 사이클이 끝났는지 확인
        float currentCycleDuration = _isDay ? DayDurationSeconds : NightDurationSeconds;
        if (_timer >= currentCycleDuration)
        {
            // 타이머 초기화 및 상태 전환
            _timer = 0;
            _isDay = !_isDay;

            // 상태 전환에 따른 시그널 방송
            if (_isDay)
            {
                EmitSignal(SignalName.DayStarted);
                GD.Print("해가 떴습니다. 낮이 시작됩니다.");
            }
            else
            {
                EmitSignal(SignalName.NightStarted);
                GD.Print("해가 졌습니다. 밤이 시작됩니다.");
            }
        }
    }
}