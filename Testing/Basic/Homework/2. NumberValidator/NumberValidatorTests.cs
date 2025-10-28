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
    
    [TestCase("12.34", TestName = "DotSeparator")]
    [TestCase("12,34", TestName = "CommaSeparator")]
    public void IsValidNumber_WithValidStringFormat_ShouldReturnTrue(string value)
    {
        var validator = new NumberValidator(10, 5);
        
        var result = validator.IsValidNumber(value);
        
        result.Should().BeTrue();
    }
    
    [TestCase("", TestName = "EmptyString")]
    [TestCase(null, TestName = "NullString")]
    [TestCase(" ", TestName = "WhiteSpaceString")]
    [TestCase("a.sd", TestName = "NotNumber")]
    [TestCase("12..34", TestName = "MultipleSeparators")]
    [TestCase("++12.34", TestName = "MultipleSigns")]
    [TestCase("12.", TestName = "FractionPartIsMissing")]
    [TestCase(".34", TestName = "IntegerPartIsMissing")]
    public void IsValidNumber_WithInvalidStringFormat_ShouldReturnFalse(string value)
    {
        var validator = new NumberValidator(10, 5);
        
        var result = validator.IsValidNumber(value);
        
        result.Should().BeFalse();
    }
}
