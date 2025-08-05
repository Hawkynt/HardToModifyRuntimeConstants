using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HardToModifyRuntimeConstants;

public static class DecimalConstants 
{
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct DecimalStorage(int lo, int mid, int hi, int flags)
    {
        public readonly int Lo = lo;
        public readonly int Mid = mid;
        public readonly int Hi = hi;
        public readonly int Flags = flags;
    }

    private readonly struct ConstantContainer() 
    {
        // Decimal constants - using actual decimal values and obfuscating them
        public readonly DecimalStorage PiDecimal = CreateObfuscatedDecimal(3.1415926535897932384626433833m);
        public readonly DecimalStorage EDecimal = CreateObfuscatedDecimal(2.7182818284590452353602874714m);
        public readonly DecimalStorage OnePercent = CreateObfuscatedDecimal(0.01m);
        
        private static DecimalStorage CreateObfuscatedDecimal(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            return new DecimalStorage(~bits[0], ~bits[1], ~bits[2], ~bits[3]);
        }
    }

    private static readonly long _storage;
    private static readonly long _storageKeyA = Random.Shared.NextInt64();
    private static readonly long _storageKeyB = Random.Shared.NextInt64();
    private const long _pepper = unchecked((long)0xfeedbeefdeadcafe);

    static DecimalConstants() 
    {
        ConstantContainer container = new();
        var pointer = GCHandle.Alloc(container, GCHandleType.Pinned).AddrOfPinnedObject().ToInt64();
        _storage = pointer ^ _storageKeyA ^ _storageKeyB ^ _pepper;
    }

    public static unsafe decimal PiDecimal => 
        DeobfuscateDecimal(((ConstantContainer*)(_storage ^ _storageKeyA ^ _storageKeyB ^ _pepper))->PiDecimal);

    public static unsafe decimal EDecimal => 
        DeobfuscateDecimal(((ConstantContainer*)(_storage ^ _storageKeyA ^ _storageKeyB ^ _pepper))->EDecimal);

    public static unsafe decimal OnePercent => 
        DeobfuscateDecimal(((ConstantContainer*)(_storage ^ _storageKeyA ^ _storageKeyB ^ _pepper))->OnePercent);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static decimal DeobfuscateDecimal(DecimalStorage obfuscated)
    {
        int[] bits = [~obfuscated.Lo, ~obfuscated.Mid, ~obfuscated.Hi, ~obfuscated.Flags];
        return new decimal(bits);
    }
}