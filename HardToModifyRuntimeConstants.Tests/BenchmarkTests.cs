using HardToModifyRuntimeConstants;
using System.Diagnostics;

namespace HardToModifyRuntimeConstants.Tests;

[TestFixture]
public class BenchmarkTests
{
    private const int IterationCount = 1_000_000;

    [Test]
    public void Benchmark_BasicConstantsVsObfuscatedConstants()
    {
        // Warm up JIT
        for (int i = 0; i < 1000; i++)
        {
            _ = Math.PI;
            _ = Constants.Pi;
        }

        // Benchmark Math.PI (baseline)
        var sw1 = Stopwatch.StartNew();
        double sum1 = 0;
        for (int i = 0; i < IterationCount; i++)
        {
            sum1 += Math.PI;
        }
        sw1.Stop();

        // Benchmark obfuscated Pi
        var sw2 = Stopwatch.StartNew();
        double sum2 = 0;
        for (int i = 0; i < IterationCount; i++)
        {
            sum2 += Constants.Pi;
        }
        sw2.Stop();

        // Verify results are equivalent
        Assert.That(sum2, Is.EqualTo(sum1).Within(1e-10));

        // Performance should be reasonable (less than 10x slower)
        double overhead = (double)sw2.ElapsedTicks / sw1.ElapsedTicks;
        
        // Output timing information
        Debug.WriteLine($"Math.PI: {sw1.ElapsedMilliseconds}ms");
        Debug.WriteLine($"Constants.Pi: {sw2.ElapsedMilliseconds}ms");
        Debug.WriteLine($"Overhead: {overhead:F2}x");
        
        Assert.That(overhead, Is.LessThan(10.0), $"Overhead too high: {overhead:F2}x");
    }

    [Test]
    public void Benchmark_EnhancedConstantsPerformance()
    {
        // Warm up
        for (int i = 0; i < 1000; i++)
        {
            _ = EnhancedDoubleConstants.Pi;
            _ = EnhancedDoubleConstants.Answer;
        }

        var sw = Stopwatch.StartNew();
        double sum = 0;
        int intSum = 0;
        
        for (int i = 0; i < IterationCount; i++)
        {
            sum += EnhancedDoubleConstants.Pi + EnhancedDoubleConstants.E + EnhancedDoubleConstants.Sqrt2 + EnhancedDoubleConstants.GoldenRatio;
            intSum += EnhancedDoubleConstants.MaxInt32 + EnhancedDoubleConstants.Answer;
        }
        
        sw.Stop();

        // Verify we got reasonable results
        Assert.That(sum, Is.GreaterThan(0));
        Assert.That(intSum, Is.GreaterThan(0));
        
        Debug.WriteLine($"Enhanced constants: {sw.ElapsedMilliseconds}ms for {IterationCount} iterations");
        
        // Should complete within reasonable time (less than 1 second for 1M iterations)
        Assert.That(sw.ElapsedMilliseconds, Is.LessThan(1000), $"Too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Test]
    public void Benchmark_DecimalConstantsPerformance()
    {
        // Warm up
        for (int i = 0; i < 1000; i++)
        {
            _ = EnhancedDecimalConstants.PiDecimal;
        }

        var sw = Stopwatch.StartNew();
        decimal sum = 0;
        
        for (int i = 0; i < IterationCount / 10; i++) // Fewer iterations for decimal operations
        {
            sum += EnhancedDecimalConstants.PiDecimal + EnhancedDecimalConstants.EDecimal + EnhancedDecimalConstants.OnePercent;
        }
        
        sw.Stop();

        Assert.That(sum, Is.GreaterThan(0));
        
        Debug.WriteLine($"Decimal constants: {sw.ElapsedMilliseconds}ms for {IterationCount / 10} iterations");
        
        // Decimal operations are naturally slower, allow more time
        Assert.That(sw.ElapsedMilliseconds, Is.LessThan(2000), $"Too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Test]
    public void Benchmark_ConcurrentAccess()
    {
        int numTasks = Environment.ProcessorCount;
        int iterationsPerTask = IterationCount / numTasks;

        var sw = Stopwatch.StartNew();
        
        var tasks = new Task[numTasks];
        for (int t = 0; t < numTasks; t++)
        {
            tasks[t] = Task.Run(() =>
            {
                double sum = 0;
                for (int i = 0; i < iterationsPerTask; i++)
                {
                    sum += Constants.Pi + EnhancedDoubleConstants.E;
                }
                return sum;
            });
        }

        Task.WaitAll(tasks);
        sw.Stop();

        Debug.WriteLine($"Concurrent access ({numTasks} tasks): {sw.ElapsedMilliseconds}ms");
        
        // Should scale reasonably with multiple cores
        Assert.That(sw.ElapsedMilliseconds, Is.LessThan(1000), $"Concurrent access too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Test]
    public void Benchmark_MemoryUsage()
    {
        // Force garbage collection before measurement
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        long beforeMemory = GC.GetTotalMemory(false);
        
        // Access all constants to ensure they're initialized
        _ = Constants.Pi;
        _ = Constants.E;
        _ = Constants.Sqrt2;
        _ = EnhancedDoubleConstants.Pi;
        _ = EnhancedDoubleConstants.E;
        _ = EnhancedDoubleConstants.Sqrt2;
        _ = EnhancedDoubleConstants.GoldenRatio;
        _ = EnhancedDoubleConstants.MaxInt32;
        _ = EnhancedDoubleConstants.Answer;
        _ = EnhancedDecimalConstants.PiDecimal;
        _ = EnhancedDecimalConstants.EDecimal;
        _ = EnhancedDecimalConstants.OnePercent;
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        long afterMemory = GC.GetTotalMemory(false);
        long memoryUsed = afterMemory - beforeMemory;
        
        Debug.WriteLine($"Memory used by constants: {memoryUsed} bytes");
        
        // Should use reasonable amount of memory (less than 10KB)
        Assert.That(memoryUsed, Is.LessThan(10240), $"Memory usage too high: {memoryUsed} bytes");
    }
}