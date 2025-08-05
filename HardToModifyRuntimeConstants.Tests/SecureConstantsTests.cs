using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

public class SecureConstantsTests
{
    [Fact]
    public void SecureConstants_ShouldReturnCorrectMathematicalValues()
    {
        Assert.Equal(Math.PI, SecureConstants.Pi, precision: 15);
        Assert.Equal(Math.E, SecureConstants.E, precision: 15);
        Assert.Equal(Math.Sqrt(2), SecureConstants.Sqrt2, precision: 15);
        
        // Golden ratio: (1 + √5) / 2
        double expectedGoldenRatio = (1 + Math.Sqrt(5)) / 2;
        Assert.Equal(expectedGoldenRatio, SecureConstants.GoldenRatio, precision: 15);
    }

    [Fact]
    public void SecureConstants_ShouldReturnCorrectIntegerValues()
    {
        Assert.Equal(int.MaxValue, SecureConstants.MaxInt32);
        Assert.Equal(42, SecureConstants.Answer);
    }

    [Fact]
    public void SecureConstants_ShouldReturnCorrectDecimalValues()
    {
        // Test decimal constants with high precision
        decimal expectedPi = 3.1415926535897932384626433833m;
        decimal expectedE = 2.7182818284590452353602874714m;
        decimal expectedOnePercent = 0.01m;

        Assert.Equal(expectedPi, SecureConstants.PiDecimal);
        Assert.Equal(expectedE, SecureConstants.EDecimal);
        Assert.Equal(expectedOnePercent, SecureConstants.OnePercent);
    }

    [Fact]
    public void SecureConstants_ShouldBeConsistent()
    {
        // Test multiple calls return same values
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(Math.PI, SecureConstants.Pi, precision: 15);
            Assert.Equal(42, SecureConstants.Answer);
            Assert.Equal(0.01m, SecureConstants.OnePercent);
        }
    }

    [Fact]
    public void SecureConstants_ShouldNotContainPlainTextValues()
    {
        // This test verifies that the generated constants don't contain plain text values
        Type constantsType = typeof(SecureConstants);
        FieldInfo[] fields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        
        // Check that we have obfuscated storage
        var containerFields = fields.Where(f => f.FieldType.Name.Contains("ConstantContainer")).ToArray();
        Assert.NotEmpty(containerFields);
        
        // The container should contain obfuscated values only
        var containerType = constantsType.GetNestedTypes(BindingFlags.NonPublic).FirstOrDefault(t => t.Name.Contains("ConstantContainer"));
        Assert.NotNull(containerType);
        
        var containerFields2 = containerType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in containerFields2)
        {
            // All fields should be ulong or uint (obfuscated values)
            Assert.True(field.FieldType == typeof(ulong) || field.FieldType == typeof(uint));
        }
    }

    [Fact]
    public void SecureConstants_ShouldHaveUniqueObfuscatedValues()
    {
        // Verify that different constants have different obfuscated values
        // This ensures the obfuscation is working properly
        
        Type constantsType = typeof(SecureConstants);
        var containerType = constantsType.GetNestedTypes(BindingFlags.NonPublic).FirstOrDefault(t => t.Name.Contains("ConstantContainer"));
        Assert.NotNull(containerType);
        
        var instance = Activator.CreateInstance(containerType);
        var fields = containerType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        var values = new List<object>();
        foreach (var field in fields)
        {
            var value = field.GetValue(instance);
            Assert.NotNull(value);
            
            // Ensure this value is unique (no duplicates)
            Assert.DoesNotContain<object>(value, values);
            values.Add(value);
        }
        
        // We should have multiple unique obfuscated values
        Assert.True(values.Count >= 10); // Pi, E, Sqrt2, GoldenRatio, MaxInt32, Answer + decimal parts
    }

    [Fact]
    public void SecureConstants_ShouldHaveRandomizedKeys()
    {
        // Verify that the keys are randomized and not hardcoded
        Type constantsType = typeof(SecureConstants);
        var storageKeyField = constantsType.GetField("_storageKey", BindingFlags.NonPublic | BindingFlags.Static);
        var sessionKeyField = constantsType.GetField("_sessionKey", BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(storageKeyField);
        Assert.NotNull(sessionKeyField);
        
        long storageKey = (long)storageKeyField.GetValue(null);
        long sessionKey = (long)sessionKeyField.GetValue(null);
        
        // Keys should not be zero (extremely unlikely with cryptographic randomness)
        Assert.NotEqual(0L, storageKey);
        Assert.NotEqual(0L, sessionKey);
        
        // Keys should be different from each other
        Assert.NotEqual(storageKey, sessionKey);
        
        // Keys should not be simple patterns
        Assert.NotEqual(0x1234567812345678L, storageKey);
        Assert.NotEqual(0xABCDEFABCDEFABCDL, sessionKey);
    }

    [Fact]
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
                Assert.Equal(expectedPi, taskResults[i], precision: 15);
                Assert.Equal(expectedE, taskResults[i + 1], precision: 15);
                Assert.Equal(expectedSqrt2, taskResults[i + 2], precision: 15);
                Assert.Equal(expectedGoldenRatio, taskResults[i + 3], precision: 15);
            }
        }
    }

    [Fact]
    public void SecureConstants_ShouldHaveProperPrecision()
    {
        // Test precision against known high-precision values
        const double HIGH_PRECISION_PI = 3.141592653589793;
        const double HIGH_PRECISION_E = 2.718281828459045;
        const double HIGH_PRECISION_SQRT2 = 1.4142135623730951;
        const double HIGH_PRECISION_GOLDEN_RATIO = 1.6180339887498948;
        
        Assert.Equal(HIGH_PRECISION_PI, SecureConstants.Pi, precision: 15);
        Assert.Equal(HIGH_PRECISION_E, SecureConstants.E, precision: 15);
        Assert.Equal(HIGH_PRECISION_SQRT2, SecureConstants.Sqrt2, precision: 15);
        Assert.Equal(HIGH_PRECISION_GOLDEN_RATIO, SecureConstants.GoldenRatio, precision: 15);
    }
}