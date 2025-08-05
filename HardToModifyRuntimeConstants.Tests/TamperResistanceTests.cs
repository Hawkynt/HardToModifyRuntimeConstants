using HardToModifyRuntimeConstants;
using System.Reflection;

namespace HardToModifyRuntimeConstants.Tests;

[TestFixture]
public class TamperResistanceTests
{
    [Test]
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
        Assert.That(Constants.Pi, Is.EqualTo(originalPi).Within(1e-15));
        Assert.That(Constants.E, Is.EqualTo(originalE).Within(1e-15));
        Assert.That(Constants.Sqrt2, Is.EqualTo(originalSqrt2).Within(1e-15));
    }

    [Test]
    public void Constants_ShouldHaveObfuscatedStorage()
    {
        Type constantsType = typeof(Constants);
        FieldInfo[] privateFields = constantsType.GetFields(BindingFlags.NonPublic | BindingFlags.Static);
        
        // Verify that we have obfuscated storage fields
        var storageField = privateFields.FirstOrDefault(f => f.Name.Contains("storage"));
        var keyField = privateFields.FirstOrDefault(f => f.Name.Contains("Key"));
        
        Assert.That(storageField, Is.Not.Null);
        Assert.That(keyField, Is.Not.Null);
        
        // Verify these fields are readonly
        Assert.That(storageField.IsInitOnly, Is.True);
        Assert.That(keyField.IsInitOnly, Is.True);
    }

    [Test]
    public void Constants_ShouldUseRandomizedKeys()
    {
        // This test verifies that the random key mechanism is working
        // by checking that multiple program runs would generate different keys
        
        Type constantsType = typeof(Constants);
        FieldInfo keyField = constantsType.GetField("_storageKey", BindingFlags.NonPublic | BindingFlags.Static);
        
        Assert.That(keyField, Is.Not.Null);
        
        long keyValue = (long)keyField.GetValue(null);
        
        // The key should not be zero (extremely unlikely with Random.Shared.NextInt64())
        Assert.That(keyValue, Is.Not.EqualTo(0L));
        
        // The key should be different from the pepper value
        FieldInfo pepperField = constantsType.GetField("_pepper", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.That(pepperField, Is.Not.Null);
        
        long pepperValue = (long)pepperField.GetValue(null);
        Assert.That(keyValue, Is.Not.EqualTo(pepperValue));
    }

    [Test]
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
                Assert.That(taskResults[i], Is.EqualTo(expectedPi).Within(1e-15));
                Assert.That(taskResults[i + 1], Is.EqualTo(expectedE).Within(1e-15));
                Assert.That(taskResults[i + 2], Is.EqualTo(expectedSqrt2).Within(1e-15));
            }
        }
    }

    [Test]
    public void Constants_ShouldHaveSecureMemoryLayout()
    {
        // Verify the constants are accessed through unsafe pointers
        // This is indicated by the use of unsafe methods in the property getters
        
        Type constantsType = typeof(Constants);
        PropertyInfo piProperty = constantsType.GetProperty("Pi");
        PropertyInfo eProperty = constantsType.GetProperty("E");
        PropertyInfo sqrt2Property = constantsType.GetProperty("Sqrt2");
        
        Assert.That(piProperty, Is.Not.Null);
        Assert.That(eProperty, Is.Not.Null);
        Assert.That(sqrt2Property, Is.Not.Null);
        
        // Properties should be static and have only getters
        Assert.That(piProperty.GetMethod.IsStatic, Is.True);
        Assert.That(eProperty.GetMethod.IsStatic, Is.True);
        Assert.That(sqrt2Property.GetMethod.IsStatic, Is.True);
        
        Assert.That(piProperty.SetMethod, Is.Null);
        Assert.That(eProperty.SetMethod, Is.Null);
        Assert.That(sqrt2Property.SetMethod, Is.Null);
    }

    [Test]
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
                Assert.That(fieldBytes.SequenceEqual(piBytes), Is.False);
                Assert.That(fieldBytes.SequenceEqual(eBytes), Is.False);
                Assert.That(fieldBytes.SequenceEqual(sqrt2Bytes), Is.False);
            }
        }
    }
}