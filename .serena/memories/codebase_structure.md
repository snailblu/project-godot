# 코드베이스 구조

## 루트 디렉토리 구조
```
project-godot/
├── .serena/              # Serena 설정
├── .claude/              # Claude Code 설정
├── .vscode/              # VS Code 설정
├── Tests/                # NUnit 테스트 파일들
├── CLAUDE.md             # Claude Code 지침서
├── .editorconfig         # 에디터 설정
├── .gitignore           # Git 무시 파일 목록
├── .gitattributes       # Git 속성 설정
├── project.godot        # Godot 프로젝트 설정
├── project godot.csproj # C# 프로젝트 파일
├── project godot.sln    # Visual Studio 솔루션
├── Main.tscn            # 메인 씬 파일
├── Player.tscn          # 플레이어 씬 파일
├── Player.cs            # 플레이어 메인 클래스
├── PlayerData.cs        # 플레이어 데이터 클래스
├── PlayerMovement.cs    # 플레이어 이동 로직 클래스
├── *.uid                # Godot 고유 ID 파일들
└── icon.svg             # 프로젝트 아이콘
```

## 주요 C# 파일
1. **Player.cs**: CharacterBody2D를 상속받는 메인 플레이어 클래스
2. **PlayerData.cs**: 플레이어 관련 데이터 관리
3. **PlayerMovement.cs**: 이동 로직 처리

## 테스트 구조
```
Tests/
├── PlayerDataTests.cs     # PlayerData 클래스 테스트
├── PlayerMovementTests.cs # PlayerMovement 클래스 테스트
└── *.uid                  # Godot 고유 ID 파일들
```

## Godot 특화 파일들
- **project.godot**: Godot 엔진 프로젝트 설정
- ***.tscn**: Godot 씬 파일들
- ***.uid**: Godot 리소스 고유 ID 파일들
- **icon.svg**: 프로젝트 아이콘

## 설정 파일들
- **.editorconfig**: 기본 에디터 설정 (UTF-8 인코딩)
- **project godot.csproj**: .NET 8.0, NUnit 패키지 참조
- **.gitignore**: Godot 4+ 전용 무시 설정