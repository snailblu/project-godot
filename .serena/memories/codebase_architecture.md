# 코드베이스 아키텍처

## 레이어드 아키텍처 개요

이 프로젝트는 **3계층 아키텍처**를 따르며, 각 계층은 명확한 책임과 의존성 규칙을 가집니다.

### 1. 비즈니스 로직 레이어 (Godot 독립적)
**특징**: 순수 C# 클래스, Godot API 접근 금지, 테스트 가능

**주요 클래스들**:
- `GameController`: 게임 흐름 제어 및 웨이브 관리 
- `WaveManager`: 웨이브 로직 관리
- `ScoreManager`: 점수 계산 로직
- `HealthComponent`: 체력 관리 컴포넌트
- `WeaponComponent`: 무기 로직 컴포넌트
- `DashLogic`: 대시 기능 순수 로직
- `PowerupLogic`: 파워업 효과 로직

**위치**: `Scripts/Components/`, `Scripts/Managers/` 내 Logic 클래스들

### 2. 헬퍼 및 유틸리티 레이어
**특징**: 공통 기능 제공, 레이어 간 공유

**주요 클래스들**:
- `GameConstants`: 모든 게임 상수 중앙 관리 (매직 넘버 제거)
- `SpawnHelper`: 스폰 위치 계산 로직
- `ValidationHelper`: 공통 유효성 검사
- `GodotLogger`: 테스트 환경 안전 로깅 (`SafePrint()` 메서드)

**위치**: `Scripts/Constants/`, `Scripts/Helpers/`, `Scripts/Utils/`

### 3. Godot 통합 레이어 (Godot 의존적)
**특징**: Godot Node 상속, UI 및 씬 관리

**주요 클래스들**:
- `GameManager`: Godot 씬과 비즈니스 로직 연결
- `Player`: 플레이어 캐릭터 (CharacterBody2D)
- `Zombie`: 좀비 엔티티들 (CharacterBody2D)
- `Events`: AutoLoad 전역 이벤트 버스
- UI 클래스들: `MainMenu`, `GameOverScreen`, `HUD`, `PauseMenu`

**위치**: `Scripts/Player/`, `Scripts/Zombies/`, `Scripts/UI/`, `Scripts/Systems/`

## 디렉토리 구조

```
Scripts/
├── Components/         # 재사용 가능한 컴포넌트들
├── Constants/          # GameConstants 및 상수 정의
├── Effects/           # 시각적 효과 (HitEffect 등)
├── Helpers/           # 헬퍼 유틸리티 클래스들
├── Interfaces/        # 인터페이스 정의 (IZombieSpawner, ISceneFactory)
├── Items/            # 게임 아이템들 (Powerup)
├── Managers/         # 게임 매니저들
├── Player/           # 플레이어 관련 클래스들
├── Systems/          # 시스템 레벨 클래스들 (Events, GameData)
├── UI/               # UI 관련 클래스들
├── Utils/            # 유틸리티 클래스들
├── Zombies/          # 좀비 관련 클래스들

Scenes/
├── UI/               # UI 씬들
├── Main/             # 메인 게임 씬들
├── Player/           # 플레이어 씬
├── Zombies/          # 좀비 씬들
├── Projectiles/      # 투사체 씬들
├── Items/            # 아이템 씬들
├── Effects/          # 효과 씬들

Tests/                # NUnit 테스트 파일들 (99개 테스트)
```

## AutoLoad 싱글톤 시스템

Godot의 AutoLoad 시스템을 통해 전역 접근 가능한 싱글톤들:

1. **Events** (`Scripts/Systems/Events.cs`)
   - 전역 이벤트 버스
   - 모든 게임 이벤트 신호 관리

2. **SoundManager** (`Scripts/Managers/SoundManager.cs`)
   - 사운드 효과 관리

3. **GameData** (`Scripts/Systems/GameData.cs`)
   - 씬 간 데이터 전달 (점수, 웨이브, 체력)

4. **SceneManager** (`Scripts/Managers/SceneManager.cs`)
   - 씬 전환 관리

## 의존성 주입 패턴

**생성자 주입 예시**:
```csharp
public GameController(WaveManager waveManager, IZombieSpawner zombieSpawner)
{
    _waveManager = waveManager;
    _zombieSpawner = zombieSpawner;
}
```

**AutoLoad 접근 (Godot 레이어에서만)**:
```csharp
var events = GetNode<Events>("/root/Events");
var gameData = GetNode<GameData>("/root/GameData");
```