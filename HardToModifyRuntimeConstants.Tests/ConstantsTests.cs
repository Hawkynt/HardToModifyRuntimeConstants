using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

public class ConstantsTests
{
    [Fact]
    public void Pi_ShouldReturnCorrectValue()
    {
        double expected = Math.PI;
        double actual = Constants.Pi;
        
        Assert.Equal(expected, actual, precision: 15);
    }

    [Fact]
    public void E_ShouldReturnCorrectValue()
    {
        double expected = Math.E;
        double actual = Constants.E;
        
        Assert.Equal(expected, actual, precision: 15);
    }

    [Fact]
    public void Sqrt2_ShouldReturnCorrectValue()
    {
        double expected = Math.Sqrt(2);
        double actual = Constants.Sqrt2;
        
        Assert.Equal(expected, actual, precision: 15);
    }

    [Fact]
    public void Constants_ShouldReturnConsistentValues()
    {
        // Test that multiple calls return the same value
        double pi1 = Constants.Pi;
        double pi2 = Constants.Pi;
        double pi3 = Constants.Pi;
        
        Assert.Equal(pi1, pi2);
        Assert.Equal(pi2, pi3);
        
        double e1 = Constants.E;
        double e2 = Constants.E;
        double e3 = Constants.E;
        
        Assert.Equal(e1, e2);
        Assert.Equal(e2, e3);
        
        double sqrt2_1 = Constants.Sqrt2;
        double sqrt2_2 = Constants.Sqrt2;
        double sqrt2_3 = Constants.Sqrt2;
        
        Assert.Equal(sqrt2_1, sqrt2_2);
        Assert.Equal(sqrt2_2, sqrt2_3);
    }

    [Fact]
    public void Constants_ShouldBeReadOnly()
    {
        // Verify that the Constants class cannot be instantiated
        var constructors = typeof(Constants).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.Empty(constructors);
        
        // Verify that the class is static
        Assert.True(typeof(Constants).IsAbstract && typeof(Constants).IsSealed);
    }

    [Fact]
    public void Constants_ShouldHaveCorrectPrecision()
    {
        // Test precision against known high-precision values
        const double HIGH_PRECISION_PI = 3.141592653589793;
        const double HIGH_PRECISION_E = 2.718281828459045;
        const double HIGH_PRECISION_SQRT2 = 1.4142135623730951;
        
        Assert.Equal(HIGH_PRECISION_PI, Constants.Pi, precision: 15);
        Assert.Equal(HIGH_PRECISION_E, Constants.E, precision: 15);
        Assert.Equal(HIGH_PRECISION_SQRT2, Constants.Sqrt2, precision: 15);
    }

    [Fact]
    public void Constants_ShouldNotBeNaN()
    {
        Assert.False(double.IsNaN(Constants.Pi));
        Assert.False(double.IsNaN(Constants.E));
        Assert.False(double.IsNaN(Constants.Sqrt2));
    }

    [Fact]
    public void Constants_ShouldNotBeInfinity()
    {
        Assert.False(double.IsInfinity(Constants.Pi));
        Assert.False(double.IsInfinity(Constants.E));
        Assert.False(double.IsInfinity(Constants.Sqrt2));
    }

    [Fact]
    public void Constants_ShouldHavePositiveValues()
    {
        Assert.True(Constants.Pi > 0);
        Assert.True(Constants.E > 0);
        Assert.True(Constants.Sqrt2 > 0);
    }

    [Fact]
    public void Constants_ShouldHaveExpectedRanges()
    {
        // Pi should be between 3 and 4
        Assert.InRange(Constants.Pi, 3.0, 4.0);
        
        // E should be between 2 and 3
        Assert.InRange(Constants.E, 2.0, 3.0);
        
        // Sqrt(2) should be between 1 and 2
        Assert.InRange(Constants.Sqrt2, 1.0, 2.0);
    }
}