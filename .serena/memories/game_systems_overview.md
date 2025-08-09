# 게임 시스템 개요

## 웨이브 시스템

### 웨이브 구성 규칙
- **기본 좀비**: 항상 3마리 (모든 웨이브)
- **러너 좀비**: 웨이브 번호만큼 (1웨이브=1마리, 2웨이브=2마리, ...)
- **탱커 좀비**: 5의 배수 웨이브마다 등장 (`GameConstants.Wave.TANK_ZOMBIE_WAVE_INTERVAL`)

### 웨이브 매니저
- `WaveManager`: 웨이브 로직 관리 (순수 C# 클래스)
- `GameController`: 웨이브 실행 및 좀비 스폰 제어

## 충돌 레이어 시스템

### 레이어 구조
- **Layer 1**: Player (CharacterBody2D: layer=1, mask=0)
- **Layer 2**: Zombies (CharacterBody2D: layer=2, mask=1)  
- **Layer 3**: Projectiles (Area2D: layer=3, mask=2)

### DamageArea 설정
- **Player DamageArea**: layer=0, mask=2 (좀비만 감지)
- **충돌 규칙**: 총알-플레이어 충돌 방지, 좀비-플레이어 물리 충돌 허용

## 이벤트 시스템 (Events.cs)

### 게임플레이 이벤트
- `ZombieDied(int scoreValue)`: 좀비 사망 시 점수 전달
- `PlayerHealthChanged(int currentHealth, int maxHealth)`: 플레이어 체력 변화
- `WaveChanged(int waveNumber)`: 웨이브 변경
- `ScoreChanged(int newScore)`: 점수 변경
- `PowerupCollected()`: 파워업 수집
- `PlayerFiredWeapon()`: 무기 발사
- `ZombieTookDamage()`, `PlayerTookDamage()`: 데미지 이벤트

### UI 이벤트
- `StartGameRequested()`: 게임 시작 요청
- `ShowSettingsRequested()`: 설정 화면 요청
- `QuitGameRequested()`: 게임 종료 요청
- `GameOver()`: 게임 오버
- `PauseToggled(bool isPaused)`: 일시정지 토글
- `ShowMainMenuRequested()`: 메인 메뉴 요청

### 사용법
```csharp
// 이벤트 발생
events.EmitSignal(Events.SignalName.ZombieDied, scoreValue);

// 이벤트 수신 (GameManager에서)
events.ZombieDied += OnZombieDied;
```

## 스폰 시스템

### SpawnHelper 클래스
- `GetRandomizedSpawnPosition()`: 랜덤 오프셋 적용 스폰 위치
- **랜덤 범위**: `GameConstants.Spawn.RANDOM_OFFSET_RANGE` (±50픽셀)
- **목적**: 동일한 위치 스폰 방지, 자연스러운 분산 효과

## 씬 관리 시스템

### 씬 구조
- **MainMenu.tscn**: 메인 메뉴 (프로젝트 시작 씬)
- **Game.tscn**: 게임플레이 씬 (이전 Main.tscn)
- **GameOverScreen.tscn**: 게임 오버 씬

### AutoLoad 싱글톤들
- **Events**: 전역 이벤트 버스
- **SoundManager**: 사운드 관리
- **GameData**: 씬 간 데이터 전달 (점수, 웨이브, 체력)
- **SceneManager**: 씬 전환 관리

### 게임 플로우
1. **게임 시작** → MainMenu.tscn 표시
2. **Start 버튼** → GameData 초기화 + Game.tscn으로 전환
3. **플레이어 사망** → GameOver.tscn으로 전환
4. **Restart/Main Menu** → 해당 씬으로 전환

## 플레이어 시스템

### 플레이어 기능
- **이동**: `PlayerMovement` 컴포넌트
- **사격**: `WeaponComponent` 컴포넌트
- **대시**: `DashComponent` + `DashLogic` (비즈니스 로직 분리)
- **체력**: `HealthComponent`

### 입력 시스템
- **이동**: 방향키 또는 WASD
- **사격**: 마우스 좌클릭 (`shoot` 액션)
- **대시**: Space 키 (`dash` 액션)

## 좀비 시스템

### 좀비 타입
- **기본 좀비** (Zombie.tscn): 기본 체력 및 속도
- **러너 좀비** (RunnerZombie.tscn): 빠른 속도
- **탱커 좀비** (TankZombie.tscn): 높은 체력

### AI 시스템
- `ZombieAIComponent`: 좀비 AI 로직
- 플레이어 추적 및 공격 패턴
- 체력 및 데미지 시스템 통합

## 파워업 시스템

### PowerupLogic (비즈니스 로직)
- 데미지 배수 증가: `DEFAULT_DAMAGE_MULTIPLIER = 2.0f`
- 지속 시간: `DEFAULT_DURATION = 5.0f`
- 순수 C# 로직, 테스트 가능

### Powerup (Godot 통합)
- PowerupLogic 래핑
- 충돌 감지 및 이벤트 발생