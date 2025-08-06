# 프로젝트 개요

## 프로젝트 목적
이 프로젝트는 Godot 4.4.1 엔진과 C#을 사용한 2D 게임 개발 프로젝트입니다. 플레이어 캐릭터의 이동 기능을 구현하며, TDD(테스트 주도 개발) 방법론을 적용하여 개발됩니다.

## 기술 스택
- **게임 엔진**: Godot 4.4.1
- **프로그래밍 언어**: C# (.NET 8.0)
- **테스트 프레임워크**: NUnit 4.0.1
- **타겟 플랫폼**: .NET 8.0

## 프로젝트 설정
- **프로젝트 이름**: "project godot"
- **어셈블리 이름**: "project godot"
- **Root Namespace**: `projectgodot`
- **메인 씬**: `res://Main.tscn`
- **Godot SDK**: Godot.NET.Sdk/4.4.1

## 핵심 구성요소
1. **Player.cs**: 메인 플레이어 캐릭터 클래스 (CharacterBody2D 상속)
2. **PlayerData.cs**: 플레이어 데이터 관리 클래스
3. **PlayerMovement.cs**: 플레이어 이동 로직 클래스
4. **Tests/**: NUnit 테스트 파일들

## 개발 접근법
- TDD(테스트 주도 개발) 방법론 사용
- 컴포지션 패턴 적용 (Player 클래스가 PlayerData, PlayerMovement 인스턴스를 포함)