using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(1, 0, TestName = "PositivePrecision_ScaleIsZero")]
    [TestCase(2, 1, TestName = "PositivePrecisionAndScale_ScaleLessThanPrecision")]
    public void Ctor_WithValidParameters_ShouldNotThrow(int precision, int scale)
    {
        var act = () => new NumberValidator(precision, scale);

        act.Should().NotThrow();
    }
    
    [TestCase(-1, 2, TestName = "NegativePrecision")]
    [TestCase(0, 1, TestName = "ZeroPrecision")]
    [TestCase(1, -1, TestName = "NegativeScale")]
    [TestCase(3, 4, TestName = "ScaleGreaterThanPrecision")]
    [TestCase(3, 3, TestName = "ScaleEqualsPrecision")]
    public void Ctor_WithInvalidParameters_ShouldThrow(int precision, int scale)    
    {
        var act = () => new NumberValidator(precision, scale);

        act.Should().Throw<ArgumentException>();
    }

}