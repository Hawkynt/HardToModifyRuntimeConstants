using HardToModifyRuntimeConstants;
using System.Diagnostics;

namespace HardToModifyRuntimeConstants.Tests;

public class BenchmarkTests
{
    private const int IterationCount = 1_000_000;

    [Fact]
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
        Assert.Equal(sum1, sum2, precision: 10);

        // Performance should be reasonable (less than 10x slower)
        double overhead = (double)sw2.ElapsedTicks / sw1.ElapsedTicks;
        
        // Output timing information
        Debug.WriteLine($"Math.PI: {sw1.ElapsedMilliseconds}ms");
        Debug.WriteLine($"Constants.Pi: {sw2.ElapsedMilliseconds}ms");
        Debug.WriteLine($"Overhead: {overhead:F2}x");
        
        Assert.True(overhead < 10.0, $"Overhead too high: {overhead:F2}x");
    }

    [Fact]
    public void Benchmark_EnhancedConstantsPerformance()
    {
        // Warm up
        for (int i = 0; i < 1000; i++)
        {
            _ = EnhancedConstants.Pi;
            _ = EnhancedConstants.Answer;
        }

        var sw = Stopwatch.StartNew();
        double sum = 0;
        int intSum = 0;
        
        for (int i = 0; i < IterationCount; i++)
        {
            sum += EnhancedConstants.Pi + EnhancedConstants.E + EnhancedConstants.Sqrt2 + EnhancedConstants.GoldenRatio;
            intSum += EnhancedConstants.MaxInt32 + EnhancedConstants.Answer;
        }
        
        sw.Stop();

        // Verify we got reasonable results
        Assert.True(sum > 0);
        Assert.True(intSum > 0);
        
        Debug.WriteLine($"Enhanced constants: {sw.ElapsedMilliseconds}ms for {IterationCount} iterations");
        
        // Should complete within reasonable time (less than 1 second for 1M iterations)
        Assert.True(sw.ElapsedMilliseconds < 1000, $"Too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void Benchmark_DecimalConstantsPerformance()
    {
        // Warm up
        for (int i = 0; i < 1000; i++)
        {
            _ = DecimalConstants.PiDecimal;
        }

        var sw = Stopwatch.StartNew();
        decimal sum = 0;
        
        for (int i = 0; i < IterationCount / 10; i++) // Fewer iterations for decimal operations
        {
            sum += DecimalConstants.PiDecimal + DecimalConstants.EDecimal + DecimalConstants.OnePercent;
        }
        
        sw.Stop();

        Assert.True(sum > 0);
        
        Debug.WriteLine($"Decimal constants: {sw.ElapsedMilliseconds}ms for {IterationCount / 10} iterations");
        
        // Decimal operations are naturally slower, allow more time
        Assert.True(sw.ElapsedMilliseconds < 2000, $"Too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
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
                    sum += Constants.Pi + EnhancedConstants.E;
                }
                return sum;
            });
        }

        Task.WaitAll(tasks);
        sw.Stop();

        Debug.WriteLine($"Concurrent access ({numTasks} tasks): {sw.ElapsedMilliseconds}ms");
        
        // Should scale reasonably with multiple cores
        Assert.True(sw.ElapsedMilliseconds < 1000, $"Concurrent access too slow: {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
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
        _ = EnhancedConstants.Pi;
        _ = EnhancedConstants.E;
        _ = EnhancedConstants.Sqrt2;
        _ = EnhancedConstants.GoldenRatio;
        _ = EnhancedConstants.MaxInt32;
        _ = EnhancedConstants.Answer;
        _ = DecimalConstants.PiDecimal;
        _ = DecimalConstants.EDecimal;
        _ = DecimalConstants.OnePercent;
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        long afterMemory = GC.GetTotalMemory(false);
        long memoryUsed = afterMemory - beforeMemory;
        
        Debug.WriteLine($"Memory used by constants: {memoryUsed} bytes");
        
        // Should use reasonable amount of memory (less than 10KB)
        Assert.True(memoryUsed < 10240, $"Memory usage too high: {memoryUsed} bytes");
    }
}