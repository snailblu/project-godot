# 코드 스타일 및 컨벤션

## C# 코딩 스타일
- **클래스 선언**: `public partial class` 형태로 선언 (Godot 노드 상속 시)
- **네임스페이스**: `projectgodot` 사용
- **접근 제한자**: private 필드는 `_` 접두사 사용 (예: `_playerData`)

## Godot 특화 컨벤션
- **노드 클래스**: Godot 노드를 상속받는 클래스는 `partial` 키워드 사용
- **라이프사이클 메서드**: 
  - `_Ready()`: 초기화 로직
  - `_PhysicsProcess(double delta)`: 물리 업데이트 로직
- **입력 처리**: `Input.GetVector()` 사용하여 방향 입력 처리

## 아키텍처 패턴
- **컴포지션 패턴**: 기능별로 클래스를 분리하고 조합하여 사용
  - `Player` 클래스가 `PlayerData`, `PlayerMovement` 인스턴스를 포함
- **책임 분리**: 각 클래스는 단일 책임을 가짐
  - `PlayerData`: 데이터 관리
  - `PlayerMovement`: 이동 로직
  - `Player`: Godot 노드와의 연결점

## 테스트 코드 스타일
- **테스트 프레임워크**: NUnit 사용
- **테스트 파일 위치**: `Tests/` 폴더
- **네임스페이스**: `projectgodot.Tests`

## 주석 및 문서화
- 한국어 주석 사용
- TDD 방법론에 대한 언급을 코드 주석에 포함