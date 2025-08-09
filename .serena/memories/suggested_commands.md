# 개발 명령어 가이드

## 필수 개발 명령어

### NuGet 패키지 관리
```bash
# NuGet 패키지 복원 (프로젝트 설정 후 첫 번째 실행 필수)
dotnet restore
```

### 테스트 실행
```bash
# 전체 테스트 실행 (99개 테스트)
TEST_ENVIRONMENT=true dotnet test

# 콘솔 로그와 함께 테스트 실행 (더 자세한 출력)
TEST_ENVIRONMENT=true dotnet test --logger console

# 특정 클래스의 테스트만 실행
TEST_ENVIRONMENT=true dotnet test --filter "TestClass=HealthComponentTests"

# 조용한 모드로 테스트 실행 (결과만 표시)
TEST_ENVIRONMENT=true dotnet test --verbosity quiet

# 사용 가능한 테스트 목록 확인
TEST_ENVIRONMENT=true dotnet test --list-tests
```

### Godot 엔진 명령어
```bash
# 헤드리스 모드에서 C# 솔루션 빌드
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions

# 헤드리스 모드로 프로젝트 초기화
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --quit

# Godot 에디터 실행 (GUI 모드)
/Applications/Godot_mono.app/Contents/MacOS/Godot

# 또는 간단히 (PATH에 등록된 경우)
godot --headless --build-solutions
godot --headless --quit
```

### .NET 개발 도구
```bash
# .NET 버전 확인
dotnet --version

# 프로젝트 빌드
dotnet build

# 프로젝트 정리
dotnet clean
```

### Git 및 일반 개발 도구
```bash
# Git 상태 확인
git status

# 변경사항 확인
git diff

# 파일 검색 (macOS)
find . -name "*.cs" | head -10

# 코드 내용 검색 (macOS)
grep -r "GameConstants" Scripts/

# 디렉토리 구조 확인
ls -la
tree (tree 명령어 설치된 경우)
```

## 태스크 완료 후 실행 권장 명령어

```bash
# 1. 모든 테스트 실행하여 회귀 테스트
TEST_ENVIRONMENT=true dotnet test

# 2. 솔루션 빌드하여 컴파일 오류 확인
dotnet build

# 3. Git 상태 확인
git status
```

## 중요 참고사항

- **TEST_ENVIRONMENT=true**: 이 환경 변수는 테스트 실행 시 필수입니다. Godot API 호출을 안전하게 우회합니다.
- **macOS 특화**: 이 프로젝트는 macOS(Darwin) 환경에서 개발되므로 Godot 경로가 `/Applications/` 하위에 있습니다.
- **테스트 우선**: 새로운 기능 개발 시 항상 테스트를 먼저 작성하고 실행하는 TDD 접근법을 따릅니다.