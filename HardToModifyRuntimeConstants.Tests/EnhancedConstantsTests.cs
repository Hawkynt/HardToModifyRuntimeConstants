using HardToModifyRuntimeConstants;

namespace HardToModifyRuntimeConstants.Tests;

public class EnhancedConstantsTests
{
    [Fact]
    public void EnhancedConstants_ShouldReturnCorrectMathematicalValues()
    {
        Assert.Equal(Math.PI, EnhancedConstants.Pi, precision: 15);
        Assert.Equal(Math.E, EnhancedConstants.E, precision: 15);
        Assert.Equal(Math.Sqrt(2), EnhancedConstants.Sqrt2, precision: 15);
        
        // Golden ratio: (1 + √5) / 2
        double expectedGoldenRatio = (1 + Math.Sqrt(5)) / 2;
        Assert.Equal(expectedGoldenRatio, EnhancedConstants.GoldenRatio, precision: 15);
    }

    [Fact]
    public void EnhancedConstants_ShouldReturnCorrectIntegerValues()
    {
        Assert.Equal(int.MaxValue, EnhancedConstants.MaxInt32);
        Assert.Equal(42, EnhancedConstants.Answer);
    }

    [Fact]
    public void EnhancedConstants_ShouldBeConsistent()
    {
        // Test multiple calls return same values
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(Math.PI, EnhancedConstants.Pi, precision: 15);
            Assert.Equal(42, EnhancedConstants.Answer);
        }
    }

    [Fact]
    public void EnhancedConstants_ShouldNotBeNaNOrInfinity()
    {
        Assert.False(double.IsNaN(EnhancedConstants.Pi));
        Assert.False(double.IsNaN(EnhancedConstants.E));
        Assert.False(double.IsNaN(EnhancedConstants.Sqrt2));
        Assert.False(double.IsNaN(EnhancedConstants.GoldenRatio));

        Assert.False(double.IsInfinity(EnhancedConstants.Pi));
        Assert.False(double.IsInfinity(EnhancedConstants.E));
        Assert.False(double.IsInfinity(EnhancedConstants.Sqrt2));
        Assert.False(double.IsInfinity(EnhancedConstants.GoldenRatio));
    }

    [Fact]
    public void EnhancedConstants_ShouldHavePositiveValues()
    {
        Assert.True(EnhancedConstants.Pi > 0);
        Assert.True(EnhancedConstants.E > 0);
        Assert.True(EnhancedConstants.Sqrt2 > 0);
        Assert.True(EnhancedConstants.GoldenRatio > 0);
        Assert.True(EnhancedConstants.MaxInt32 > 0);
        Assert.True(EnhancedConstants.Answer > 0);
    }

    [Fact]
    public void DecimalConstants_ShouldReturnCorrectValues()
    {
        // Test decimal constants with high precision
        decimal expectedPi = 3.1415926535897932384626433833m;
        decimal expectedE = 2.7182818284590452353602874714m;
        decimal expectedOnePercent = 0.01m;

        Assert.Equal(expectedPi, DecimalConstants.PiDecimal);
        Assert.Equal(expectedE, DecimalConstants.EDecimal);
        Assert.Equal(expectedOnePercent, DecimalConstants.OnePercent);
    }

    [Fact]
    public void DecimalConstants_ShouldMaintainPrecision()
    {
        // Test that decimal constants maintain their precision
        decimal pi = DecimalConstants.PiDecimal;
        string piString = pi.ToString("F28", System.Globalization.CultureInfo.InvariantCulture);
        
        Assert.Contains("3.1415926535897932384626433833", piString);
    }

    [Fact]
    public void DecimalConstants_ShouldBeConsistent()
    {
        decimal pi1 = DecimalConstants.PiDecimal;
        decimal pi2 = DecimalConstants.PiDecimal;
        decimal e1 = DecimalConstants.EDecimal;
        decimal e2 = DecimalConstants.EDecimal;

        Assert.Equal(pi1, pi2);
        Assert.Equal(e1, e2);
    }
}