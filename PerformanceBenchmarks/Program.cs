using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HardToModifyRuntimeConstants;
using System.Security.Cryptography;

namespace PerformanceBenchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class ConstantAccessBenchmarks
{
    [Benchmark(Description = "Level 1: Basic Constants", Baseline = true)]
    public double AccessBasicConstants()
    {
        // Access basic constants (Level 1)
        return Constants.Pi + Constants.E + Constants.Sqrt2;
    }

    [Benchmark(Description = "Level 2: Enhanced Constants")]
    public double AccessEnhancedConstants()
    {
        // Access enhanced constants (Level 2)
        return EnhancedConstants.Pi + EnhancedConstants.E + EnhancedConstants.Sqrt2 + EnhancedConstants.GoldenRatio;
    }

    [Benchmark(Description = "Level 3: Compile-Time Obfuscated")]
    public double AccessSecureConstants()
    {
        // Access compile-time obfuscated constants (Level 3)
        return SecureConstants.Pi + SecureConstants.E + SecureConstants.Sqrt2 + SecureConstants.GoldenRatio;
    }

    [Benchmark(Description = "Level 4: Crypto Constants (Exception Test)")]
    public int AccessCryptoConstants()
    {
        // Level 4 constants throw exceptions by design
        int exceptionCount = 0;
        try { var _ = CryptoConstants.Pi; } catch (CryptographicException) { exceptionCount++; }
        try { var _ = CryptoConstants.E; } catch (CryptographicException) { exceptionCount++; }
        return exceptionCount;
    }
}

[MemoryDiagnoser]
[SimpleJob]
public class IntegerAccessBenchmarks
{
    [Benchmark(Description = "Level 2: Enhanced Integer Access", Baseline = true)]
    public int AccessEnhancedIntegers()
    {
        return EnhancedConstants.MaxInt32 + EnhancedConstants.Answer;
    }

    [Benchmark(Description = "Level 3: Secure Integer Access")]
    public int AccessSecureIntegers()
    {
        return SecureConstants.MaxInt32 + SecureConstants.Answer;
    }
}

[MemoryDiagnoser]
[SimpleJob]
public class DecimalAccessBenchmarks
{
    [Benchmark(Description = "Level 2: Enhanced Decimal Access", Baseline = true)]
    public decimal AccessEnhancedDecimals()
    {
        return DecimalConstants.PiDecimal + DecimalConstants.EDecimal + DecimalConstants.OnePercent;
    }

    [Benchmark(Description = "Level 3: Secure Decimal Access")]
    public decimal AccessSecureDecimals()
    {
        try
        {
            return SecureConstants.PiDecimal + SecureConstants.EDecimal + SecureConstants.OnePercent;
        }
        catch
        {
            // Return fallback value if deobfuscation fails
            return 0m;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🚀 Hard-to-Modify Runtime Constants - Performance Benchmarks");
        Console.WriteLine("============================================================");
        Console.WriteLine();
        Console.WriteLine("Running benchmarks to compare performance across all 4 security levels...");
        Console.WriteLine();

        // Run all benchmarks
        BenchmarkRunner.Run<ConstantAccessBenchmarks>();
        BenchmarkRunner.Run<IntegerAccessBenchmarks>();
        BenchmarkRunner.Run<DecimalAccessBenchmarks>();

        Console.WriteLine();
        Console.WriteLine("Benchmark Summary:");
        Console.WriteLine("- Level 1 (Basic): Direct constant access - fastest");
        Console.WriteLine("- Level 2 (Enhanced): Runtime XOR obfuscation - moderate overhead");
        Console.WriteLine("- Level 3 (Secure): Compile-time complex obfuscation - higher overhead");
        Console.WriteLine("- Level 4 (Crypto): Asymmetric encryption - exceptions by design");
    }
}
