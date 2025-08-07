# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

이 프로젝트는 C# Godot 프로젝트입니다. Godot 4.4.1을 사용하며 .NET 8.0을 타겟으로 합니다.

## 개발 명령어

### 빌드 및 테스트

- `dotnet restore` - NuGet 패키지 복원
- `dotnet test` - NUnit 테스트 실행
- `dotnet test --logger console` - 콘솔 로그와 함께 테스트 실행

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

테스트 파일들은 `Tests/` 폴더에 위치하며, NUnit 프레임워크를 사용합니다. Godot 엔진이 헤드리스 모드에서 C# 솔루션을 빌드한 후 `dotnet test` 명령으로 테스트를 실행할 수 있습니다.

### Godot 특수 사항

- 프로젝트 파일에 공백이 포함된 이름("project godot")을 사용합니다
- C# 스크립트는 `public partial class`로 작성되며 Godot 노드를 상속받습니다
- 메인 씬(`Main.tscn`)이 프로젝트 실행을 위해 설정되어 있습니다

- 항상 컴포지션 철학에 맞게 디자인해 주세요.
- 새로운 피처를 추가할 때는 TDD 접근법을 사용해 주세요.
- 씬을 작성할때는 직접 tscn 파일을 작성해 주세요.
