// HealthComponent.cs (Godot Node 래퍼)
using Godot;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);
    [Signal]
    public delegate void DiedEventHandler();

    [Export]
    private int _maxHealth = 100;

    private HealthLogic _logic;

    public override void _Ready()
    {
        // 1. 순수 로직 객체 생성
        _logic = new HealthLogic(_maxHealth);

        // 2. 로직의 이벤트를 Godot의 시그널로 연결 (구독)
        _logic.HealthChanged += (current, max) => EmitSignal(SignalName.HealthChanged, current, max);
        _logic.Died += () => EmitSignal(SignalName.Died);
    }

    // 3. 외부 호출을 내부 로직으로 전달
    public void TakeDamage(float amount)
    {
        _logic.TakeDamage(amount);
    }
    
    // 외부에서 정보가 필요할 경우를 대비해 속성 제공
    public float CurrentHealth => _logic.CurrentHealth;
    public bool IsDead => _logic.IsDead;
}