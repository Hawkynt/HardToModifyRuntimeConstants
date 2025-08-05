using HardToModifyRuntimeConstants;

namespace HardToModifyRuntimeConstants.Tests;

[TestFixture]
public class EnhancedConstantsTests
{
    [Test]
    public void EnhancedConstants_ShouldReturnCorrectMathematicalValues()
    {
        Assert.That(EnhancedDoubleConstants.Pi, Is.EqualTo(Math.PI).Within(1e-15));
        Assert.That(EnhancedDoubleConstants.E, Is.EqualTo(Math.E).Within(1e-15));
        Assert.That(EnhancedDoubleConstants.Sqrt2, Is.EqualTo(Math.Sqrt(2)).Within(1e-15));
        
        // Golden ratio: (1 + √5) / 2
        double expectedGoldenRatio = (1 + Math.Sqrt(5)) / 2;
        Assert.That(EnhancedDoubleConstants.GoldenRatio, Is.EqualTo(expectedGoldenRatio).Within(1e-15));
    }

    [Test]
    public void EnhancedConstants_ShouldReturnCorrectIntegerValues()
    {
        Assert.That(EnhancedDoubleConstants.MaxInt32, Is.EqualTo(int.MaxValue));
        Assert.That(EnhancedDoubleConstants.Answer, Is.EqualTo(42));
    }

    [Test]
    public void EnhancedConstants_ShouldBeConsistent()
    {
        // Test multiple calls return same values
        for (int i = 0; i < 100; i++)
        {
            Assert.That(EnhancedDoubleConstants.Pi, Is.EqualTo(Math.PI).Within(1e-15));
            Assert.That(EnhancedDoubleConstants.Answer, Is.EqualTo(42));
        }
    }

    [Test]
    public void EnhancedConstants_ShouldNotBeNaNOrInfinity()
    {
        Assert.That(double.IsNaN(EnhancedDoubleConstants.Pi), Is.False);
        Assert.That(double.IsNaN(EnhancedDoubleConstants.E), Is.False);
        Assert.That(double.IsNaN(EnhancedDoubleConstants.Sqrt2), Is.False);
        Assert.That(double.IsNaN(EnhancedDoubleConstants.GoldenRatio), Is.False);

        Assert.That(double.IsInfinity(EnhancedDoubleConstants.Pi), Is.False);
        Assert.That(double.IsInfinity(EnhancedDoubleConstants.E), Is.False);
        Assert.That(double.IsInfinity(EnhancedDoubleConstants.Sqrt2), Is.False);
        Assert.That(double.IsInfinity(EnhancedDoubleConstants.GoldenRatio), Is.False);
    }

    [Test]
    public void EnhancedConstants_ShouldHavePositiveValues()
    {
        Assert.That(EnhancedDoubleConstants.Pi, Is.GreaterThan(0));
        Assert.That(EnhancedDoubleConstants.E, Is.GreaterThan(0));
        Assert.That(EnhancedDoubleConstants.Sqrt2, Is.GreaterThan(0));
        Assert.That(EnhancedDoubleConstants.GoldenRatio, Is.GreaterThan(0));
        Assert.That(EnhancedDoubleConstants.MaxInt32, Is.GreaterThan(0));
        Assert.That(EnhancedDoubleConstants.Answer, Is.GreaterThan(0));
    }

    [Test]
    public void DecimalConstants_ShouldReturnCorrectValues()
    {
        // Test decimal constants with high precision
        decimal expectedPi = 3.1415926535897932384626433833m;
        decimal expectedE = 2.7182818284590452353602874714m;
        decimal expectedOnePercent = 0.01m;

        Assert.That(EnhancedDecimalConstants.PiDecimal, Is.EqualTo(expectedPi));
        Assert.That(EnhancedDecimalConstants.EDecimal, Is.EqualTo(expectedE));
        Assert.That(EnhancedDecimalConstants.OnePercent, Is.EqualTo(expectedOnePercent));
    }

    [Test]
    public void DecimalConstants_ShouldMaintainPrecision()
    {
        // Test that decimal constants maintain their precision
        decimal pi = EnhancedDecimalConstants.PiDecimal;
        string piString = pi.ToString("F28", System.Globalization.CultureInfo.InvariantCulture);
        
        Assert.That(piString, Does.Contain("3.1415926535897932384626433833"));
    }

    [Test]
    public void DecimalConstants_ShouldBeConsistent()
    {
        decimal pi1 = EnhancedDecimalConstants.PiDecimal;
        decimal pi2 = EnhancedDecimalConstants.PiDecimal;
        decimal e1 = EnhancedDecimalConstants.EDecimal;
        decimal e2 = EnhancedDecimalConstants.EDecimal;

        Assert.That(pi2, Is.EqualTo(pi1));
        Assert.That(e2, Is.EqualTo(e1));
    }
}