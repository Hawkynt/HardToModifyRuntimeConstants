using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

[TestFixture]
public class SecureConstantsTests
{
    [Test]
    public void SecureConstants_ShouldReturnCorrectMathematicalValues()
    {
        Assert.That(SecureConstants.Pi, Is.EqualTo(Math.PI).Within(1e-15));
        Assert.That(SecureConstants.E, Is.EqualTo(Math.E).Within(1e-15));
        Assert.That(SecureConstants.Sqrt2, Is.EqualTo(Math.Sqrt(2)).Within(1e-15));
        
        // Golden ratio: (1 + √5) / 2
        double expectedGoldenRatio = (1 + Math.Sqrt(5)) / 2;
        Assert.That(SecureConstants.GoldenRatio, Is.EqualTo(expectedGoldenRatio).Within(1e-15));
    }

    [Test]
    public void SecureConstants_ShouldReturnCorrectIntegerValues()
    {
        Assert.That(SecureConstants.MaxInt32, Is.EqualTo(int.MaxValue));
        Assert.That(SecureConstants.Answer, Is.EqualTo(42));
    }

    [Test]
    public void SecureConstants_ShouldReturnCorrectDecimalValues()
    {
        // Test decimal constants with high precision
        decimal expectedPi = 3.1415926535897932384626433833m;
        decimal expectedE = 2.7182818284590452353602874714m;
        decimal expectedOnePercent = 0.01m;

        Assert.That(SecureConstants.PiDecimal, Is.EqualTo(expectedPi));
        Assert.That(SecureConstants.EDecimal, Is.EqualTo(expectedE));
        Assert.That(SecureConstants.OnePercent, Is.EqualTo(expectedOnePercent));
    }

    [Test]
    public void SecureConstants_ShouldBeConsistent()
    {
        // Test multiple calls return same values
        for (int i = 0; i < 100; i++)
        {
            Assert.That(SecureConstants.Pi, Is.EqualTo(Math.PI).Within(1e-15));
            Assert.That(SecureConstants.Answer, Is.EqualTo(42));
            Assert.That(SecureConstants.OnePercent, Is.EqualTo(0.01m));
        }
    }

    [Test]
    public void SecureConstants_ShouldNotContainPlainTextValues()
    {
        // This test verifies that the generated constants don't contain plain text values
        Type constantsType = typeof(SecureConstants);
        FieldInfo[] fields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        
        // Check that we have obfuscated storage
        var containerFields = fields.Where(f => f.FieldType.Name.Contains("ConstantContainer")).ToArray();
        Assert.That(containerFields, Is.Not.Empty);
        
        // The container should contain obfuscated values only
        var containerType = constantsType.GetNestedTypes(BindingFlags.NonPublic).FirstOrDefault(t => t.Name.Contains("ConstantContainer"));
        Assert.That(containerType, Is.Not.Null);
        
        var containerFields2 = containerType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in containerFields2)
        {
            // All fields should be ulong or uint (obfuscated values)
            Assert.That(field.FieldType == typeof(ulong) || field.FieldType == typeof(uint), Is.True);
        }
    }

    [Test]
    public void SecureConstants_ShouldHaveUniqueObfuscatedValues()
    {
        // Verify that different constants have different obfuscated values
        // This ensures the obfuscation is working properly
        
        Type constantsType = typeof(SecureConstants);
        var containerType = constantsType.GetNestedTypes(BindingFlags.NonPublic).FirstOrDefault(t => t.Name.Contains("ConstantContainer"));
        Assert.That(containerType, Is.Not.Null);
        
        var instance = Activator.CreateInstance(containerType);
        var fields = containerType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        var values = new List<object>();
        foreach (var field in fields)
        {
            var value = field.GetValue(instance);
            Assert.That(value, Is.Not.Null);
            
            // Ensure this value is unique (no duplicates)
            Assert.That(values, Does.Not.Contain(value));
            values.Add(value);
        }
        
        // We should have multiple unique obfuscated values
        Assert.That(values.Count, Is.GreaterThanOrEqualTo(10)); // Pi, E, Sqrt2, GoldenRatio, MaxInt32, Answer + decimal parts
    }

    [Test]
    public void SecureConstants_ShouldHaveRandomizedKeys()
    {
        // Verify that the keys are randomized and not hardcoded
        Type constantsType = typeof(SecureConstants);
        var storageKeyField = constantsType.GetField("_storageKey", BindingFlags.NonPublic | BindingFlags.Static);
        var sessionKeyField = constantsType.GetField("_sessionKey", BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.That(storageKeyField, Is.Not.Null);
        Assert.That(sessionKeyField, Is.Not.Null);
        
        long storageKey = (long)storageKeyField.GetValue(null);
        long sessionKey = (long)sessionKeyField.GetValue(null);
        
        // Keys should not be zero (extremely unlikely with cryptographic randomness)
        Assert.That(storageKey, Is.Not.EqualTo(0L));
        Assert.That(sessionKey, Is.Not.EqualTo(0L));
        
        // Keys should be different from each other
        Assert.That(sessionKey, Is.Not.EqualTo(storageKey));
        
        // Keys should not be simple patterns
        Assert.That(storageKey, Is.Not.EqualTo(0x1234567812345678L));
        Assert.That(sessionKey, Is.Not.EqualTo(0xABCDEFABCDEFABCDL));
    }

    [Test]
    public void SecureConstants_ShouldMaintainConsistencyUnderConcurrentAccess()
    {
        const int numTasks = 10;
        const int numIterations = 1000;
        
        var tasks = new Task[numTasks];
        var results = new double[numTasks][];
        
        for (int i = 0; i < numTasks; i++)
        {
            int taskIndex = i;
            tasks[i] = Task.Run(() =>
            {
                results[taskIndex] = new double[numIterations * 4];
                for (int j = 0; j < numIterations; j++)
                {
                    results[taskIndex][j * 4] = SecureConstants.Pi;
                    results[taskIndex][j * 4 + 1] = SecureConstants.E;
                    results[taskIndex][j * 4 + 2] = SecureConstants.Sqrt2;
                    results[taskIndex][j * 4 + 3] = SecureConstants.GoldenRatio;
                }
            });
        }
        
        Task.WaitAll(tasks);
        
        // Verify all tasks got the same values
        double expectedPi = SecureConstants.Pi;
        double expectedE = SecureConstants.E;
        double expectedSqrt2 = SecureConstants.Sqrt2;
        double expectedGoldenRatio = SecureConstants.GoldenRatio;
        
        foreach (var taskResults in results)
        {
            for (int i = 0; i < taskResults.Length; i += 4)
            {
                Assert.That(taskResults[i], Is.EqualTo(expectedPi).Within(1e-15));
                Assert.That(taskResults[i + 1], Is.EqualTo(expectedE).Within(1e-15));
                Assert.That(taskResults[i + 2], Is.EqualTo(expectedSqrt2).Within(1e-15));
                Assert.That(taskResults[i + 3], Is.EqualTo(expectedGoldenRatio).Within(1e-15));
            }
        }
    }

    [Test]
    public void SecureConstants_ShouldHaveProperPrecision()
    {
        // Test precision against known high-precision values
        const double HIGH_PRECISION_PI = 3.141592653589793;
        const double HIGH_PRECISION_E = 2.718281828459045;
        const double HIGH_PRECISION_SQRT2 = 1.4142135623730951;
        const double HIGH_PRECISION_GOLDEN_RATIO = 1.6180339887498948;
        
        Assert.That(SecureConstants.Pi, Is.EqualTo(HIGH_PRECISION_PI).Within(1e-15));
        Assert.That(SecureConstants.E, Is.EqualTo(HIGH_PRECISION_E).Within(1e-15));
        Assert.That(SecureConstants.Sqrt2, Is.EqualTo(HIGH_PRECISION_SQRT2).Within(1e-15));
        Assert.That(SecureConstants.GoldenRatio, Is.EqualTo(HIGH_PRECISION_GOLDEN_RATIO).Within(1e-15));
    }
}