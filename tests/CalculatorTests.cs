namespace Tests;

using NUnit.Framework;
using projectgodot;

[TestFixture] // 이 클래스는 테스트 코드를 담고 있는 묶음(Fixture)이라고 선언합니다.
public class CalculatorTests
{
    [Test] // 이 메서드는 하나의 독립적인 테스트 케이스라고 선언합니다.
    public void Add_TwoNumbers_ReturnsCorrectSum()
    {
        // 1. Arrange (준비): 테스트에 필요한 객체와 변수를 설정합니다.
        var calculator = new Calculator(); // 'Calculator' 클래스는 아직 존재하지 않습니다!
        int a = 5;
        int b = 10;
        int expected = 15;

        // 2. Act (실행): 테스트하고 싶은 실제 메서드를 호출합니다.
        int actual = calculator.Add(a, b); // 'Add' 메서드도 당연히 없습니다.

        // 3. Assert (단언): 실행 결과가 우리가 예상한 값과 같은지 확인합니다.
        Assert.That(actual, Is.EqualTo(expected));
    }
}