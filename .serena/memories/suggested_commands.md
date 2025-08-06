# 권장 개발 명령어

## NuGet 패키지 관리
```bash
dotnet restore
```
- NuGet 패키지들을 복원합니다.

## 테스트 실행
```bash
dotnet test
```
- NUnit 테스트를 실행합니다.

```bash
dotnet test --logger console
```
- 콘솔 로그와 함께 테스트를 실행합니다.

## Godot 엔진 명령어
```bash
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions
```
- 헤드리스 모드에서 C# 솔루션을 빌드합니다.

```bash
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --quit
```
- 헤드리스 모드로 프로젝트를 초기화합니다.

## 기본 시스템 명령어 (macOS)
```bash
ls -la          # 파일 목록 보기
cd [directory]  # 디렉토리 이동
grep [pattern]  # 텍스트 검색
find [path]     # 파일 찾기
git status      # Git 상태 확인
git add .       # 모든 변경사항 스테이징
git commit -m   # 커밋
```

## 프로젝트 실행
Godot 에디터에서 F5키 또는 Play 버튼을 통해 게임을 실행할 수 있습니다.