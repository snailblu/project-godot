# 코딩 컨벤션 및 스타일 가이드

## 네이밍 컨벤션

### 클래스 및 메서드
- **PascalCase** 사용
- 클래스: `GameController`, `HealthComponent`, `ZombieSpawner`
- 메서드: `StartWave()`, `OnZombieDied()`, `SetZombieScene()`
- 프로퍼티: `ZombiesRemainingInWave`, `IsAlive`

### 필드 및 변수
- **Private 필드**: `_camelCase` (언더스코어 접두사)
- **Local 변수**: `camelCase`
- **상수**: `UPPER_SNAKE_CASE`

**예시**:
```csharp
private WaveManager _waveManager;
private IZombieSpawner _zombieSpawner;
public const float DEFAULT_SPEED = 600.0f;
```

### 이벤트 핸들러
- **Delegate**: `[ActionName]EventHandler` 형식
- **메서드**: `On[ActionName]` 형식

**예시**:
```csharp
[Signal]
public delegate void ZombieDiedEventHandler(int scoreValue);

public void OnZombieDied(int scoreValue) { }
```

## 코드 구조 패턴

### 클래스 구조 순서
1. 필드 (private → public)
2. 생성자
3. 프로퍼티
4. 이벤트
5. Public 메서드
6. Private 메서드

### 네임스페이스 구조
- **Root**: `projectgodot`
- **서브 네임스페이스**: `projectgodot.Constants`, `projectgodot.Tests`

## 테스트 코드 컨벤션

### NUnit 스타일
- **Assert.That** 문법 사용 (구식 Assert 사용 금지)
- **테스트 메서드명**: `[Method]_[Scenario]_[ExpectedResult]` 패턴

**올바른 예시**:
```csharp
[Test]
public void Constructor_WithValidDependencies_InitializesCorrectly()
{
    // Arrange, Act, Assert 주석 사용
    Assert.That(_gameController, Is.Not.Null);
    Assert.That(_gameController.ZombiesRemainingInWave, Is.EqualTo(0));
}
```

### 모킹 패턴
```csharp
private Mock<IZombieSpawner> _mockSpawner;

[SetUp]
public void SetUp()
{
    _mockSpawner = new Mock<IZombieSpawner>();
}

// Verify 사용
_mockSpawner.Verify(
    s => s.SpawnZombie(It.IsAny<PackedScene>(), It.IsAny<Vector2>()), 
    Times.Exactly(5)
);
```

## 아키텍처 규칙

### 레이어 분리 규칙
1. **비즈니스 로직 클래스**: Godot API 접근 금지
2. **Godot 통합 클래스**: `GetNode<T>()` 방식으로 AutoLoad 접근
3. **테스트 환경**: `TEST_ENVIRONMENT=true`에서 독립 실행 가능

### 금지된 패턴
```csharp
// ❌ 잘못된 방법 (존재하지 않는 API)
var tree = Engine.GetSingleton("SceneTree") as SceneTree;

// ✅ 올바른 방법
var events = GetNode<Events>("/root/Events");
```

## 상수 관리

### GameConstants 사용 강제
- **매직 넘버 금지**: 모든 숫자는 GameConstants에서 관리
- **중첩 클래스 구조**: 카테고리별 그룹핑

**예시**:
```csharp
public static class GameConstants
{
    public static class Zombie
    {
        public const float DEFAULT_MOVE_SPEED = 900.0f;
        public const int DEFAULT_INITIAL_HEALTH = 30;
    }
    
    public static class Wave
    {
        public const int TANK_ZOMBIE_WAVE_INTERVAL = 5;
    }
}
```

## 이벤트 시스템 패턴

### 이벤트 발생
```csharp
events.EmitSignal(Events.SignalName.ZombieDied, scoreValue);
```

### 이벤트 수신
```csharp
events.ZombieDied += OnZombieDied;
```

## 로깅 패턴
```csharp
// 테스트 환경 안전 로깅
GodotLogger.SafePrint("게임 시작됨");
```

## 에러 핸들링
```csharp
// ValidationHelper 사용
ValidationHelper.ValidateNotNull(zombieScene, nameof(zombieScene));
```

## 주석 정책
- **한국어 주석**: 복잡한 비즈니스 로직 설명용
- **XML 문서화**: Public API에 대해서만 필요시
- **TODO 주석**: 향후 개선사항 표시