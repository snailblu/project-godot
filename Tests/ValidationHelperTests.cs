using NUnit.Framework;
using System.Collections.Generic;
using projectgodot.Helpers;

namespace projectgodot.Tests
{
    [TestFixture]
    public class ValidationHelperTests
    {
        [Test]
        public void IsPositiveValue_WithPositiveInt_ShouldReturnTrue()
        {
            Assert.That(ValidationHelper.IsPositiveValue(1), Is.True);
            Assert.That(ValidationHelper.IsPositiveValue(100), Is.True);
        }
        
        [Test]
        public void IsPositiveValue_WithZeroOrNegativeInt_ShouldReturnFalse()
        {
            Assert.That(ValidationHelper.IsPositiveValue(0), Is.False);
            Assert.That(ValidationHelper.IsPositiveValue(-1), Is.False);
            Assert.That(ValidationHelper.IsPositiveValue(-100), Is.False);
        }
        
        [Test]
        public void IsPositiveValue_WithPositiveFloat_ShouldReturnTrue()
        {
            Assert.That(ValidationHelper.IsPositiveValue(1.0f), Is.True);
            Assert.That(ValidationHelper.IsPositiveValue(0.1f), Is.True);
        }
        
        [Test]
        public void IsPositiveValue_WithZeroOrNegativeFloat_ShouldReturnFalse()
        {
            Assert.That(ValidationHelper.IsPositiveValue(0.0f), Is.False);
            Assert.That(ValidationHelper.IsPositiveValue(-1.0f), Is.False);
        }
        
        [Test]
        public void IsPositiveValue_WithPositiveDouble_ShouldReturnTrue()
        {
            Assert.That(ValidationHelper.IsPositiveValue(1.0), Is.True);
            Assert.That(ValidationHelper.IsPositiveValue(0.1), Is.True);
        }
        
        [Test]
        public void IsPositiveValue_WithZeroOrNegativeDouble_ShouldReturnFalse()
        {
            Assert.That(ValidationHelper.IsPositiveValue(0.0), Is.False);
            Assert.That(ValidationHelper.IsPositiveValue(-1.0), Is.False);
        }
        
        [Test]
        public void IsValidHealth_WithValidValues_ShouldReturnTrue()
        {
            Assert.That(ValidationHelper.IsValidHealth(0), Is.True);
            Assert.That(ValidationHelper.IsValidHealth(1), Is.True);
            Assert.That(ValidationHelper.IsValidHealth(100), Is.True);
        }
        
        [Test]
        public void IsValidHealth_WithNegativeValue_ShouldReturnFalse()
        {
            Assert.That(ValidationHelper.IsValidHealth(-1), Is.False);
            Assert.That(ValidationHelper.IsValidHealth(-100), Is.False);
        }
        
        [Test]
        public void IsValidMaxHealth_WithPositiveValue_ShouldReturnTrue()
        {
            Assert.That(ValidationHelper.IsValidMaxHealth(1), Is.True);
            Assert.That(ValidationHelper.IsValidMaxHealth(100), Is.True);
        }
        
        [Test]
        public void IsValidMaxHealth_WithZeroOrNegativeValue_ShouldReturnFalse()
        {
            Assert.That(ValidationHelper.IsValidMaxHealth(0), Is.False);
            Assert.That(ValidationHelper.IsValidMaxHealth(-1), Is.False);
        }
        
        [Test]
        public void IsValidCollection_WithValidCollection_ShouldReturnTrue()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.That(ValidationHelper.IsValidCollection(list), Is.True);
        }
        
        [Test]
        public void IsValidCollection_WithEmptyCollection_ShouldReturnFalse()
        {
            var list = new List<int>();
            Assert.That(ValidationHelper.IsValidCollection(list), Is.False);
        }
        
        [Test]
        public void IsValidCollection_WithNullCollection_ShouldReturnFalse()
        {
            List<int> list = null;
            Assert.That(ValidationHelper.IsValidCollection(list), Is.False);
        }
    }
}