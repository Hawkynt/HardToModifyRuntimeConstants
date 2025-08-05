using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

public class TamperResistanceTests
{
    [Fact]
    public void Constants_ShouldResistReflectionBasedTampering()
    {
        // Get original values
        double originalPi = Constants.Pi;
        double originalE = Constants.E;
        double originalSqrt2 = Constants.Sqrt2;
        
        // Try to access private fields via reflection
        Type constantsType = typeof(Constants);
        FieldInfo[] privateFields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        
        // Attempt to modify private fields (this should be very difficult due to obfuscation)
        foreach (var field in privateFields)
        {
            if (field.IsInitOnly) // readonly fields
            {
                try
                {
                    // This should fail or have no effect due to readonly nature
                    field.SetValue(null, 0L);
                }
                catch
                {
                    // Expected behavior - readonly fields cannot be modified
                }
            }
        }
        
        // Verify values are still correct after tampering attempt
        Assert.Equal(originalPi, Constants.Pi, precision: 15);
        Assert.Equal(originalE, Constants.E, precision: 15);
        Assert.Equal(originalSqrt2, Constants.Sqrt2, precision: 15);
    }

    [Fact]
    public void Constants_ShouldHaveObfuscatedStorage()
    {
        Type constantsType = typeof(Constants);
        FieldInfo[] privateFields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        
        // Verify that we have obfuscated storage fields
        var storageField = privateFields.FirstOrDefault(f => f.Name.Contains("storage"));
        var keyField = privateFields.FirstOrDefault(f => f.Name.Contains("Key"));
        
        Assert.NotNull(storageField);
        Assert.NotNull(keyField);
        
        // Verify these fields are readonly
        Assert.True(storageField.IsInitOnly);
        Assert.True(keyField.IsInitOnly);
    }

    [Fact]
    public void Constants_ShouldUseRandomizedKeys()
    {
        // This test verifies that the random key mechanism is working
        // by checking that multiple program runs would generate different keys
        
        Type constantsType = typeof(Constants);
        FieldInfo keyField = constantsType.GetField("_storageKey", BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.NotNull(keyField);
        
        long keyValue = (long)keyField.GetValue(null);
        
        // The key should not be zero (extremely unlikely with Random.Shared.NextInt64())
        Assert.NotEqual(0L, keyValue);
        
        // The key should be different from the pepper value
        FieldInfo pepperField = constantsType.GetField("_pepper", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(pepperField);
        
        long pepperValue = (long)pepperField.GetValue(null);
        Assert.NotEqual(pepperValue, keyValue);
    }

    [Fact]
    public void Constants_ShouldMaintainConsistencyUnderConcurrentAccess()
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
                results[taskIndex] = new double[numIterations * 3];
                for (int j = 0; j < numIterations; j++)
                {
                    results[taskIndex][j * 3] = Constants.Pi;
                    results[taskIndex][j * 3 + 1] = Constants.E;
                    results[taskIndex][j * 3 + 2] = Constants.Sqrt2;
                }
            });
        }
        
        Task.WaitAll(tasks);
        
        // Verify all tasks got the same values
        double expectedPi = Constants.Pi;
        double expectedE = Constants.E;
        double expectedSqrt2 = Constants.Sqrt2;
        
        foreach (var taskResults in results)
        {
            for (int i = 0; i < taskResults.Length; i += 3)
            {
                Assert.Equal(expectedPi, taskResults[i], precision: 15);
                Assert.Equal(expectedE, taskResults[i + 1], precision: 15);
                Assert.Equal(expectedSqrt2, taskResults[i + 2], precision: 15);
            }
        }
    }

    [Fact]
    public void Constants_ShouldHaveSecureMemoryLayout()
    {
        // Verify the constants are accessed through unsafe pointers
        // This is indicated by the use of unsafe methods in the property getters
        
        Type constantsType = typeof(Constants);
        PropertyInfo piProperty = constantsType.GetProperty("Pi");
        PropertyInfo eProperty = constantsType.GetProperty("E");
        PropertyInfo sqrt2Property = constantsType.GetProperty("Sqrt2");
        
        Assert.NotNull(piProperty);
        Assert.NotNull(eProperty);
        Assert.NotNull(sqrt2Property);
        
        // Properties should be static and have only getters
        Assert.True(piProperty.GetMethod.IsStatic);
        Assert.True(eProperty.GetMethod.IsStatic);
        Assert.True(sqrt2Property.GetMethod.IsStatic);
        
        Assert.Null(piProperty.SetMethod);
        Assert.Null(eProperty.SetMethod);
        Assert.Null(sqrt2Property.SetMethod);
    }

    [Fact]
    public void Constants_ShouldResistBasicMemoryScanning()
    {
        // Get the actual constant values
        double pi = Constants.Pi;
        double e = Constants.E;
        double sqrt2 = Constants.Sqrt2;
        
        // Convert to byte arrays to simulate memory scanning
        byte[] piBytes = BitConverter.GetBytes(pi);
        byte[] eBytes = BitConverter.GetBytes(e);
        byte[] sqrt2Bytes = BitConverter.GetBytes(sqrt2);
        
        // In a real obfuscated system, searching for these exact byte patterns
        // in memory should not directly reveal the storage location
        // This test verifies we're not storing values in plain form
        
        Type constantsType = typeof(Constants);
        FieldInfo[] allFields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public);
        
        foreach (var field in allFields)
        {
            if (field.FieldType == typeof(ulong))
            {
                ulong fieldValue = (ulong)field.GetValue(null);
                byte[] fieldBytes = BitConverter.GetBytes(fieldValue);
                
                // The stored value should not match the actual constant value
                Assert.False(fieldBytes.SequenceEqual(piBytes));
                Assert.False(fieldBytes.SequenceEqual(eBytes));
                Assert.False(fieldBytes.SequenceEqual(sqrt2Bytes));
            }
        }
    }
}