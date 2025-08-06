# 작업 완료 시 체크리스트

## 개발 작업 완료 후 실행할 명령어

### 1. 패키지 복원
```bash
dotnet restore
```

### 2. 테스트 실행
```bash
dotnet test
```
- 모든 테스트가 통과하는지 확인

### 3. Godot 솔루션 빌드 (필요시)
```bash
/Applications/Godot_mono.app/Contents/MacOS/Godot --headless --build-solutions
```

### 4. 코드 품질 확인
- 코딩 컨벤션 준수 확인
- 한국어 주석 작성 확인
- TDD 원칙 적용 확인

### 5. Git 작업 (필요시)
```bash
git status
git add .
git commit -m "작업 내용 설명"
```

## 주의사항
- 모든 테스트가 통과해야 함
- Godot 엔진에서 실행 가능해야 함
- 기존 기능에 영향을 주지 않아야 함
- 단순하고 최소한의 변경사항 유지

## TDD 체크포인트
1. 실패하는 테스트 작성 ✓
2. 테스트를 통과하는 최소한의 코드 작성 ✓
3. 리팩토링 및 코드 개선 ✓
4. 모든 테스트 재실행 및 통과 확인 ✓