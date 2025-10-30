using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string precisionExceptionMessage = "precision must be a positive number";
    private const string scaleExceptionMessage = "scale must be a non-negative number less or equal than precision";
    
    [TestCase(1, 0, TestName = "PositivePrecision_ScaleIsZero")]
    [TestCase(2, 1, TestName = "PositivePrecisionAndScale_ScaleLessThanPrecision")]
    public void Ctor_WithValidParameters_ShouldNotThrow(int precision, int scale)
    {
        var act = () => new NumberValidator(precision, scale);

        act.Should().NotThrow();
    }

    [TestCase(-1, 2, precisionExceptionMessage, TestName = "NegativePrecision")]
    [TestCase(0, 1,precisionExceptionMessage, TestName = "ZeroPrecision")]
    [TestCase(1, -1,scaleExceptionMessage, TestName = "NegativeScale")]
    [TestCase(3, 4, scaleExceptionMessage, TestName = "ScaleGreaterThanPrecision")]
    [TestCase(3, 3, scaleExceptionMessage, TestName = "ScaleEqualsPrecision")]
    public void Ctor_WithInvalidParameters_ShouldThrow(int precision, int scale, string message)
    {
        var act = () => new NumberValidator(precision, scale);

        act.Should().Throw<ArgumentException>()
            .WithMessage(message);
    }

    [TestCase("12.34", TestName = "DotSeparator")]
    [TestCase("12,34", TestName = "CommaSeparator")]
    [TestCase("01", TestName = "LeadingZero")]
    [TestCase("1.200", TestName = "TrailingZeros")]
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
    [TestCase("1ab.3", TestName = "DigitsAndLetters")]
    public void IsValidNumber_WithInvalidStringFormat_ShouldReturnFalse(string value)
    {
        var validator = new NumberValidator(10, 5);

        var result = validator.IsValidNumber(value);

        result.Should().BeFalse();
    }

    [TestCase("12.34", 4, 2, TestName = "ExactPrecisionAndScale")]
    [TestCase("12.3", 4, 2, TestName = "LessThanMaximumPrecision")]
    [TestCase("1234", 4, 0, TestName = "IntegerExactScale")]
    [TestCase("123.456", 6, 3, TestName = "FractionExactScale")]
    [TestCase("-123.45", 6, 3, TestName = "Negative")]
    public void IsValidNumber_WithinPrecisionAndScaleLimits_ShouldReturnTrue(string value, int precision, int scale)
    {
        var validator = new NumberValidator(precision, scale);

        var result = validator.IsValidNumber(value);

        result.Should().BeTrue();
    }

    [TestCase("123.45", 4, 2, TestName = "TotalDigitsExceedLimits")]
    [TestCase("123456", 5, 0, TestName = "IntegerExceedsLimits")]
    [TestCase("12.34", 4, 1, TestName = "FractionPartExceedsLimits")]
    [TestCase("+0.00", 3, 2, TestName = "MinusSignExceedsLimits")]
    [TestCase("+00.0", 3, 2, TestName = "PlusSignExceedsLimits")]
    public void IsValidNumber_ExceedsPrecisionAndScaleLimits_ShouldReturnFalse(string value, int precision, int scale)
    {
        var validator = new NumberValidator(precision, scale);

        var result = validator.IsValidNumber(value);

        result.Should().BeFalse();
    }
    
    [TestCase("-1234", TestName = "NegativeInteger")]
    [TestCase("-1.23", TestName = "NegativeWithFraction")]
    [TestCase("-0", TestName = "NegativeZero")]
    public void IsValidNumber_OnlyPositiveWithNegativeNumber_ShouldReturnFalse(string value)
    {
        var validator = new NumberValidator(5, 2, true);
        
        var result = validator.IsValidNumber(value);
        
        result.Should().BeFalse();
    }
}
