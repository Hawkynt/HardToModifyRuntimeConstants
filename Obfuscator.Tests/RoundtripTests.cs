using ConstantObfuscator;
using HardToModifyRuntimeConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscator.Tests;

[TestFixture]
internal class RoundtripTests
{
    private static IEnumerable<(long,string)> ParameterSource()
    {
        yield return (0L, string.Empty);
        yield return (1L, string.Empty);
        yield return (long.MaxValue, string.Empty);
        yield return (long.MinValue, string.Empty);

        yield return (0L, "test");
        yield return (1L, "test");
        yield return (long.MaxValue, "test");
        yield return (long.MinValue, "test");

        var entropy = Random.Shared;
        var buffer = new byte[4096];
        for(var i=0;i<10;++i, entropy.NextBytes(buffer))
            yield return (entropy.NextInt64(), Encoding.UTF8.GetString(buffer));

    }

    [Test]
    [TestCaseSource(nameof(ParameterSource))]
    public void TryRoundtrip64((long key, string identifier) data)
    {
        var (key, identifier) = data;
        var value = Math.PI;

        var obfuscated = Level3.ApplyComplexObfuscation(Unsafe.BitCast<double, ulong>(value), key, identifier);
        var back = Unsafe.BitCast<ulong, double>(SecureConstants.ReverseComplexObfuscation(obfuscated, key, identifier));

        Assert.That(back, Is.EqualTo(value));
    }

    [Test]
    [TestCaseSource(nameof(ParameterSource))]
    public void TryRoundtrip32((long key, string identifier) data)
    {
        var (key, identifier) = data;
        var value = 0xdeadbeefu;

        var obfuscated = Level3.ApplyComplexObfuscation32(value, key, identifier);
        var back = SecureConstants.ReverseComplexObfuscation32(obfuscated, key, identifier);

        Assert.That(back, Is.EqualTo(value));
    }
}
