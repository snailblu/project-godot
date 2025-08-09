using NUnit.Framework;
using System;
using projectgodot.Utils;

namespace projectgodot.Tests
{
    [TestFixture]
    public class GodotLoggerTests
    {
        [Test]
        public void SafePrint_InTestEnvironment_ShouldNotThrow()
        {
            // 테스트 환경에서는 예외가 발생하지 않아야 함
            Assert.That(() => GodotLogger.SafePrint("Test message"), Throws.Nothing);
        }
        
        [Test]
        public void LogDamage_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogDamage(10, 50, 100), Throws.Nothing);
        }
        
        [Test]
        public void LogDeath_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogDeath(), Throws.Nothing);
        }
        
        [Test]
        public void LogGameState_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogGameState("Game started"), Throws.Nothing);
        }
        
        [Test]
        public void LogDebug_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogDebug("Debug information"), Throws.Nothing);
        }
        
        [Test]
        public void LogDamage_WithVariousValues_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogDamage(0, 0, 100), Throws.Nothing);
            Assert.That(() => GodotLogger.LogDamage(100, 0, 100), Throws.Nothing);
            Assert.That(() => GodotLogger.LogDamage(-10, 50, 100), Throws.Nothing);
        }
        
        [Test]
        public void LogGameState_WithEmptyMessage_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogGameState(""), Throws.Nothing);
            Assert.That(() => GodotLogger.LogGameState(null), Throws.Nothing);
        }
        
        [Test]
        public void LogDebug_WithEmptyMessage_ShouldNotThrow()
        {
            Assert.That(() => GodotLogger.LogDebug(""), Throws.Nothing);
            Assert.That(() => GodotLogger.LogDebug(null), Throws.Nothing);
        }
    }
}