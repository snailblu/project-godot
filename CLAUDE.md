# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

이 프로젝트는 C# Godot 프로젝트입니다. Godot 4.4.1을 사용하며 .NET 8.0을 타겟으로 합니다.

## 개발 명령어

### 빌드 및 테스트

- `dotnet restore` - NuGet 패키지 복원
- `TEST_ENVIRONMENT=true dotnet test` - NUnit 테스트 실행
- `TEST_ENVIRONMENT=true dotnet test --logger console` - 콘솔 로그와 함께 테스트 실행

### Godot 엔진 사용

- `/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions` - 헤드리스 모드에서 C# 솔루션 빌드
- `/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --quit` - 헤드리스 모드로 프로젝트 초기화

## 프로젝트 구조

### C# 설정

- **Root Namespace**: `projectgodot`
- **Target Framework**: .NET 8.0
- **Assembly Name**: "project godot"
- **Main Scene**: `res://Main.tscn`

### 테스트 환경

- **Testing Framework**: NUnit 4.0.1
- **Test Directory**: `Tests/`
- **Test Namespace**: `projectgodot.Tests`

테스트 파일들은 `Tests/` 폴더에 위치하며, NUnit 프레임워크를 사용합니다. Godot 엔진이 헤드리스 모드에서 C# 솔루션을 빌드한 후 `TEST_ENVIRONMENT=true dotnet test` 명령으로 테스트를 실행할 수 있습니다.

**중요**: 테스트 실행 시 `TEST_ENVIRONMENT=true` 환경 변수가 필요합니다. 이는 Godot 특화 메서드(`GD.Print`, `GetNode` 등)가 테스트 환경에서 충돌을 일으키는 것을 방지합니다.

### Godot 특수 사항

- 프로젝트 파일에 공백이 포함된 이름("project godot")을 사용합니다
- C# 스크립트는 `public partial class`로 작성되며 Godot 노드를 상속받습니다
- 메인 씬(`Main.tscn`)이 프로젝트 실행을 위해 설정되어 있습니다

- 항상 컴포지션 철학에 맞게 디자인해 주세요.
- 새로운 피처를 추가할 때는 TDD 접근법을 사용해 주세요.
- 씬을 작성할때는 직접 tscn 파일을 작성해 주세요.

## 충돌 레이어 설정 (Collision Layers)

### 레이어 구조
- **Layer 1**: 플레이어 (Player)
- **Layer 2**: 좀비 (Zombies)  
- **Layer 3**: 총알 (Projectiles)

### 각 오브젝트별 설정

**Player.tscn:**
- CharacterBody2D (플레이어 이동): `collision_layer = 1`, `collision_mask = 0`
- DamageArea (Area2D, 좀비 감지용): `collision_layer = 0`, `collision_mask = 2`

**Zombie.tscn:**
- CharacterBody2D: `collision_layer = 2`, `collision_mask = 1`

**projectile.tscn:**
- Area2D: `collision_layer = 3`, `collision_mask = 2`

### 동작 원리
- 총알(Layer 3)은 좀비(Layer 2)만 감지하고 플레이어는 무시
- 플레이어의 DamageArea는 좀비(Layer 2)만 감지
- 좀비는 플레이어(Layer 1)와만 물리적 충돌
- 이렇게 설정하면 총알이 생성되자마자 플레이어와 충돌하는 문제가 해결됨
