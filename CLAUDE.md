# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

이 프로젝트는 C# Godot 4.4.1을 사용하는 좀비 디펜스 게임입니다. .NET 8.0을 타겟으로 하며, 현재 플레이어 컴포넌트 모듈화 리팩토링 중입니다. TDD와 컴포지션 패턴을 적용한 고품질 코드베이스를 목표로 합니다.

## 개발 명령어

### 빌드 및 테스트

- `dotnet restore` - NuGet 패키지 복원
- `dotnet test` - 모든 NUnit 테스트 실행
- `dotnet test --logger console` - 콘솔 로그와 함께 테스트 실행
- `dotnet test --filter "TestClass=HealthComponentTests"` - 특정 클래스 테스트만 실행

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

### 테스트 환경

- **Framework**: NUnit 4.0.1 (Assert.That 문법 사용)
- **Coverage**: NUnit 테스트로 핵심 로직 커버
- **TEST_ENVIRONMENT**: Godot API 호출을 안전하게 우회하는 환경 변수
- **Mock**: Moq 4.20.72를 사용한 의존성 모킹

### 개발 가이드라인

### C# 프로젝트 설정

- **Root Namespace**: `projectgodot`
- **Target Framework**: .NET 8.0
- **Assembly Name**: "project godot" (공백 포함)
- **Main Scene**: `res://Scenes/Main/Game.tscn` (게임플레이 씬)

### Godot API 접근 규칙

## 씬 매니징 시스템

### 씬 구조

- **Scenes/UI/MainMenu.tscn**: 메인 메뉴 (프로젝트 시작 씬)
- **Scenes/Main/Game.tscn**: 게임플레이 씬 (이전 Main.tscn)
- **Scenes/UI/GameOverScreen.tscn**: 게임 오버 씬
- **Scenes/Player/Player.tscn**: 플레이어 씬

### 디렉토리 구조

```
project-godot/
├── Scenes/               # Godot 씬 파일들
│   ├── Main/            # 메인 게임 씬들
│   ├── Player/          # 플레이어 씬
│   ├── UI/              # UI 씬들
│   ├── Zombies/         # 좀비 씬들
│   ├── Projectiles/     # 투사체 씬들
│   ├── Items/           # 아이템 씬들
│   ├── Effects/         # 효과 씬들
│   └── World/           # 월드 환경 씬들
├── Scripts/             # C# 스크립트 파일들
│   ├── Player/          # 플레이어 관련 스크립트
│   ├── Systems/         # 시스템 레벨 스크립트들
│   ├── Utils/           # 유틸리티 클래스들
│   ├── Helpers/         # 헬퍼 클래스들
│   ├── Items/           # 아이템 관련 스크립트
│   └── Zombies/         # 좀비 관련 스크립트
├── Textures/            # 게임 텍스처 및 이미지
├── Audio/               # 사운드 파일들
├── Tests/               # NUnit 테스트 파일들 (현재 비어있음)
├── .taskmaster/         # Task Master AI 설정 및 태스크
└── 기타 설정 파일들
```

## Task Master AI Instructions

**Import Task Master's development workflow commands and guidelines, treat as if import is in the main CLAUDE.md file.**
@./.taskmaster/CLAUDE.md
