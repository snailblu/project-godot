# 태스크 완료 시 체크리스트

## 필수 검증 단계

### 1. 테스트 실행 (최우선)
```bash
# 모든 테스트 실행 - 99개 테스트가 모두 통과해야 함
TEST_ENVIRONMENT=true dotnet test

# 실패 시 상세 로그와 함께 재실행
TEST_ENVIRONMENT=true dotnet test --logger console
```

**기대 결과**: `Passed! - Failed: 0, Passed: 99, Skipped: 0`

### 2. 빌드 검증
```bash
# 컴파일 오류 및 경고 확인
dotnet build

# NuGet 패키지 복원 (필요시)
dotnet restore
```

**기대 결과**: `Build succeeded. 0 Error(s), 0 Warning(s)`

### 3. Godot 프로젝트 검증
```bash
# C# 솔루션 빌드 (Godot 엔진에서)
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions
```

## 코드 품질 검증

### 1. 아키텍처 규칙 준수
- [ ] **레이어 분리**: 비즈니스 로직이 Godot API에 직접 의존하지 않음
- [ ] **상수 사용**: 매직 넘버 대신 GameConstants 사용
- [ ] **의존성 주입**: 생성자를 통한 의존성 주입 패턴 준수
- [ ] **이벤트 버스**: Events 클래스를 통한 이벤트 통신

### 2. 네이밍 컨벤션
- [ ] **클래스**: PascalCase
- [ ] **필드**: _camelCase (private), PascalCase (public)
- [ ] **메서드**: PascalCase
- [ ] **상수**: UPPER_SNAKE_CASE

### 3. 테스트 커버리지
- [ ] **새로운 비즈니스 로직**: NUnit 테스트 작성 필수
- [ ] **Assert.That 문법**: 구식 Assert 문법 사용 금지
- [ ] **Moq 사용**: 의존성 모킹 적절히 적용

## 커밋 전 최종 검토

### 1. Git 상태 확인
```bash
git status
git diff
```

### 2. 불필요한 파일 제외
- [ ] `.godot/` 디렉토리는 .gitignore에 포함
- [ ] 임시 파일, 빌드 아티팩트 제외
- [ ] 민감한 정보 (API 키, 비밀번호) 포함되지 않음

### 3. CLAUDE.md 업데이트 (필요시)
- [ ] 새로운 명령어나 워크플로우 추가 시 문서 업데이트
- [ ] 아키텍처 변경 시 해당 내용 반영

## 에러 대응 가이드

### 테스트 실패 시
1. **특정 테스트 클래스만 실행**
   ```bash
   TEST_ENVIRONMENT=true dotnet test --filter "TestClass=HealthComponentTests"
   ```

2. **상세 로그 확인**
   ```bash
   TEST_ENVIRONMENT=true dotnet test --logger console --verbosity detailed
   ```

### 빌드 실패 시
1. **Clean 후 재빌드**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Godot 솔루션 재생성**
   ```bash
   /Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions
   ```

## 성공 기준

✅ **완료 조건**:
- 모든 테스트 통과 (99/99)
- 빌드 성공 (0 Error, 0 Warning)
- 코딩 컨벤션 준수
- Git 상태 깔끔

✅ **품질 기준**:
- TDD 원칙 준수 (테스트 우선 작성)
- 레이어드 아키텍처 유지
- 이벤트 기반 통신 패턴 사용
- GameConstants 활용한 상수 관리