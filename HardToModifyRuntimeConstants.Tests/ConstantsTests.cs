using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

[TestFixture]
public class ConstantsTests
{
    [Test]
    public void Pi_ShouldReturnCorrectValue()
    {
        double expected = Math.PI;
        double actual = Constants.Pi;
        
        Assert.That(actual, Is.EqualTo(expected).Within(1e-15));
    }

    [Test]
    public void E_ShouldReturnCorrectValue()
    {
        double expected = Math.E;
        double actual = Constants.E;
        
        Assert.That(actual, Is.EqualTo(expected).Within(1e-15));
    }

    [Test]
    public void Sqrt2_ShouldReturnCorrectValue()
    {
        double expected = Math.Sqrt(2);
        double actual = Constants.Sqrt2;
        
        Assert.That(actual, Is.EqualTo(expected).Within(1e-15));
    }

    [Test]
    public void Constants_ShouldReturnConsistentValues()
    {
        // Test that multiple calls return the same value
        double pi1 = Constants.Pi;
        double pi2 = Constants.Pi;
        double pi3 = Constants.Pi;
        
        Assert.That(pi2, Is.EqualTo(pi1));
        Assert.That(pi3, Is.EqualTo(pi2));
        
        double e1 = Constants.E;
        double e2 = Constants.E;
        double e3 = Constants.E;
        
        Assert.That(e2, Is.EqualTo(e1));
        Assert.That(e3, Is.EqualTo(e2));
        
        double sqrt2_1 = Constants.Sqrt2;
        double sqrt2_2 = Constants.Sqrt2;
        double sqrt2_3 = Constants.Sqrt2;
        
        Assert.That(sqrt2_2, Is.EqualTo(sqrt2_1));
        Assert.That(sqrt2_3, Is.EqualTo(sqrt2_2));
    }

    [Test]
    public void Constants_ShouldBeReadOnly()
    {
        // Verify that the Constants class cannot be instantiated
        var constructors = typeof(Constants).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.That(constructors, Is.Empty);
        
        // Verify that the class is static
        Assert.That(typeof(Constants).IsAbstract && typeof(Constants).IsSealed, Is.True);
    }

    [Test]
    public void Constants_ShouldHaveCorrectPrecision()
    {
        // Test precision against known high-precision values
        const double HIGH_PRECISION_PI = 3.141592653589793;
        const double HIGH_PRECISION_E = 2.718281828459045;
        const double HIGH_PRECISION_SQRT2 = 1.4142135623730951;
        
        Assert.That(Constants.Pi, Is.EqualTo(HIGH_PRECISION_PI).Within(1e-15));
        Assert.That(Constants.E, Is.EqualTo(HIGH_PRECISION_E).Within(1e-15));
        Assert.That(Constants.Sqrt2, Is.EqualTo(HIGH_PRECISION_SQRT2).Within(1e-15));
    }

    [Test]
    public void Constants_ShouldNotBeNaN()
    {
        Assert.That(double.IsNaN(Constants.Pi), Is.False);
        Assert.That(double.IsNaN(Constants.E), Is.False);
        Assert.That(double.IsNaN(Constants.Sqrt2), Is.False);
    }

    [Test]
    public void Constants_ShouldNotBeInfinity()
    {
        Assert.That(double.IsInfinity(Constants.Pi), Is.False);
        Assert.That(double.IsInfinity(Constants.E), Is.False);
        Assert.That(double.IsInfinity(Constants.Sqrt2), Is.False);
    }

    [Test]
    public void Constants_ShouldHavePositiveValues()
    {
        Assert.That(Constants.Pi, Is.GreaterThan(0));
        Assert.That(Constants.E, Is.GreaterThan(0));
        Assert.That(Constants.Sqrt2, Is.GreaterThan(0));
    }

    [Test]
    public void Constants_ShouldHaveExpectedRanges()
    {
        // Pi should be between 3 and 4
        Assert.That(Constants.Pi, Is.InRange(3.0, 4.0));
        
        // E should be between 2 and 3
        Assert.That(Constants.E, Is.InRange(2.0, 3.0));
        
        // Sqrt(2) should be between 1 and 2
        Assert.That(Constants.Sqrt2, Is.InRange(1.0, 2.0));
    }
}