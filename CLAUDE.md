# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

이 프로젝트는 C# Godot 4.4.1을 사용하는 좀비 디펜스 게임입니다. .NET 8.0을 타겟으로 하며, TDD와 컴포지션 패턴을 적용한 고품질 코드베이스입니다.

## 개발 명령어

### 빌드 및 테스트

- `dotnet restore` - NuGet 패키지 복원
- `TEST_ENVIRONMENT=true dotnet test` - 모든 NUnit 테스트 실행 (105개 테스트)
- `TEST_ENVIRONMENT=true dotnet test --logger console` - 콘솔 로그와 함께 테스트 실행
- `TEST_ENVIRONMENT=true dotnet test --filter "TestClass=HealthComponentTests"` - 특정 클래스 테스트만 실행

### Godot 엔진 사용

- `/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions` - 헤드리스 모드에서 C# 솔루션 빌드
- `/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --quit` - 헤드리스 모드로 프로젝트 초기화

### 배포

- `./deploy.sh snailblu/zombie-defense-game` - itch.io에 Windows/macOS 빌드 배포
- `./deploy.sh snailblu/zombie-defense-game v1.0.0` - 특정 버전으로 배포
- **배포 전 요구사항**: `butler login` 명령어로 itch.io 계정 로그인 필요
- **게임 URL**: https://snailblu.itch.io/zombie-defense-game

## 아키텍처 및 코드 구조

### 핵심 아키텍처 원칙

- **컴포지션 패턴**: 상속보다 컴포지션을 우선적으로 사용
- **TDD 접근법**: 모든 새로운 기능은 테스트 우선 개발
- **이벤트 기반 아키텍처**: `Events` 클래스를 통한 전역 이벤트 버스
- **의존성 주입**: 생성자를 통한 의존성 주입으로 테스트 가능한 구조

### 레이어드 아키텍처

**비즈니스 로직 레이어** (테스트 가능, Godot 독립적)

- `GameController`: 게임 흐름 제어
- `WaveManager`: 웨이브 관리 로직
- `ScoreManager`: 점수 계산 로직
- `HealthComponent`, `WeaponComponent` 등: 재사용 가능한 컴포넌트들

**헬퍼 및 유틸리티 레이어**

- `GameConstants`: 모든 게임 상수 중앙 관리
- `SpawnHelper`: 스폰 위치 계산 로직
- `ValidationHelper`: 공통 유효성 검사
- `GodotLogger`: 테스트 환경 안전 로깅

**Godot 통합 레이어** (Godot 의존적)

- `GameManager`: Godot 씬과 비즈니스 로직 연결
- `Player`, `Zombie` 등: Godot 노드를 상속받는 게임 오브젝트
- `Events`: AutoLoad로 등록되는 전역 이벤트 버스

### 테스트 환경

- **Framework**: NUnit 4.0.1 (Assert.That 문법 사용)
- **Coverage**: 105개 테스트로 핵심 로직 커버
- **TEST_ENVIRONMENT**: Godot API 호출을 안전하게 우회하는 환경 변수
- **Mock**: Moq 4.20.72를 사용한 의존성 모킹

### 개발 가이드라인

**새로운 기능 개발 시:**

1. GameConstants에서 필요한 상수 정의
2. 비즈니스 로직을 순수 C# 클래스로 먼저 구현 (Godot 독립적)
3. NUnit 테스트 작성 (Assert.That 문법 사용)
4. Godot 노드와 통합하는 래퍼 클래스 구현

**코드 품질 원칙:**

- 매직 넘버 금지: GameConstants 사용
- 중복 코드 제거: Helper 클래스 활용
- 유효성 검사: ValidationHelper 사용
- 로깅: GodotLogger.SafePrint() 사용

### C# 프로젝트 설정

- **Root Namespace**: `projectgodot`
- **Target Framework**: .NET 8.0
- **Assembly Name**: "project godot" (공백 포함)
- **Main Scene**: `res://Main.tscn`

## 이벤트 기반 통신

### 전역 이벤트 버스 (Events.cs)

Godot AutoLoad로 등록된 전역 이벤트 시스템:

- `ZombieDied(int scoreValue)`: 좀비 사망 시 점수 전달
- `PlayerHealthChanged(int currentHealth, int maxHealth)`: 플레이어 체력 변화
- `WaveChanged(int waveNumber)`: 웨이브 변경
- `ScoreChanged(int newScore)`: 점수 변경
- 게임 상태 변경, 사운드 이벤트 등

### 컴포넌트 통신 패턴

**이벤트 발생**: `events.EmitSignal(Events.SignalName.ZombieDied, scoreValue)`
**이벤트 수신**: `events.ZombieDied += OnZombieDied` (GameManager에서)

## 충돌 레이어 시스템

**레이어 구조:**

- **Layer 1**: Player (CharacterBody2D: layer=1, mask=0)
- **Layer 2**: Zombies (CharacterBody2D: layer=2, mask=1)
- **Layer 3**: Projectiles (Area2D: layer=3, mask=2)

**DamageArea 설정:**

- Player DamageArea: layer=0, mask=2 (좀비만 감지)
- 총알-플레이어 충돌 방지, 좀비-플레이어 물리 충돌 허용

## 웨이브 시스템 아키텍처

### 좀비 컴포지션 패턴

- 기본 좀비: 항상 3마리
- 러너 좀비: 웨이브 번호만큼 (1웨이브=1마리, 2웨이브=2마리)
- 탱커 좀비: 5의 배수 웨이브마다 (GameConstants.Wave.TANK_ZOMBIE_WAVE_INTERVAL)

### 스폰 시스템

- SpawnHelper.GetRandomizedSpawnPosition()으로 랜덤 오프셋 적용
- GameConstants.Spawn.RANDOM_OFFSET_RANGE (±50픽셀) 내 분산

## TDD 및 아키텍처 원칙

### 레이어 분리 원칙

**비즈니스 로직 레이어** (순수 C#, Godot 독립적)
- `*Logic` 클래스들 (예: MainMenuLogic, GameOverScreenLogic)
- 순수 C# 클래스로 구현, Godot API 접근 금지
- 테스트 가능해야 하며 EnvironmentHelper 사용 불필요

**UI/Godot 통합 레이어** (Godot 의존적)
- Godot Node를 상속받는 클래스들
- `GetNode<T>("/root/NodeName")` 방식으로 AutoLoad 접근
- `Engine.GetSingleton()` 사용 금지 (존재하지 않는 API)

### Godot API 접근 규칙

**✅ 올바른 방법:**
```csharp
// Node 클래스 내에서
var events = GetNode<Events>("/root/Events");
var gameData = GetNode<GameData>("/root/GameData");
```

**❌ 잘못된 방법:**
```csharp
// Logic 클래스에서 Godot API 접근 시도
var tree = Engine.GetSingleton("SceneTree") as SceneTree; // 에러 발생
```

### 의존성 주입 패턴

Logic 클래스가 Godot 데이터에 접근해야 할 경우:
1. **Option 1**: UI 클래스에서 필요한 데이터를 Logic 클래스 메서드에 파라미터로 전달
2. **Option 2**: Logic 클래스는 순수 계산만 담당하고, UI 클래스에서 결과를 받아 Godot API 호출

### 테스트 가능한 구조 유지

- Logic 클래스는 반드시 TEST_ENVIRONMENT에서 독립적으로 실행 가능해야 함
- Godot 의존성이 있는 코드는 UI 레이어에만 위치
- Mock이나 의존성 주입을 통해 테스트 격리

## 씬 매니징 시스템

### 씬 구조
- **MainMenu.tscn**: 메인 메뉴 (프로젝트 시작 씬)
- **Game.tscn**: 게임플레이 씬 (이전 Main.tscn)
- **GameOverScreen.tscn**: 게임 오버 씬

### AutoLoad 싱글톤
- **Events**: 전역 이벤트 버스
- **SoundManager**: 사운드 관리
- **GameData**: 씬 간 데이터 전달 (점수, 웨이브, 체력)
- **SceneManager**: 씬 전환 관리

### 게임 플로우
1. 게임 시작 → MainMenu.tscn 표시
2. Start 버튼 → GameData 초기화 + Game.tscn으로 전환
3. 플레이어 사망 → GameOver.tscn으로 전환
4. Restart/Main Menu → 해당 씬으로 전환

- GodotLogger는 projectgodot.Utils 네임스페이스에 있다.