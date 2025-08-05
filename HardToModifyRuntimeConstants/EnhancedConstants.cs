using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HardToModifyRuntimeConstants;

public static class EnhancedConstants 
{
    private readonly struct ConstantContainer() 
    {
        // Values are bitwise inverted, bit-rotated, and byte-scrambled
        public readonly ulong Pi = RotateLeft(ScrambleBytes(~0x400921FB54442D18UL), 7);
        public readonly ulong E = RotateLeft(ScrambleBytes(~0x4005BF0A8B145769UL), 11);
        public readonly ulong Sqrt2 = RotateLeft(ScrambleBytes(~0x3FF6A09E667F3BCCUL), 13);
        public readonly ulong GoldenRatio = RotateLeft(ScrambleBytes(~0x3FF9E3779B97F4A8UL), 17);
        
        // Integer constants (32-bit)
        public readonly uint MaxInt32 = ScrambleBytes32(~(uint)int.MaxValue);
        public readonly uint Answer = ScrambleBytes32(~42U);
    }

    private static readonly long _storage;
    private static readonly long _storageKey = Random.Shared.NextInt64();
    private static readonly long _sessionKey = Random.Shared.NextInt64();
    private const long _pepper = unchecked((long)0xdeadbeefcaffee42);
    private const long _salt = unchecked((long)0x1337c0ffeebabeadL);

    static EnhancedConstants() 
    {
        ConstantContainer container = new();
        var pointer = GCHandle.Alloc(container, GCHandleType.Pinned).AddrOfPinnedObject().ToInt64();
        _storage = pointer ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt;
    }

    // Mathematical constants
    public static unsafe double Pi => 
        Unsafe.BitCast<ulong, double>(UnscrambleBytes(RotateRight(
            (~((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->Pi), 7)));

    public static unsafe double E => 
        Unsafe.BitCast<ulong, double>(UnscrambleBytes(RotateRight(
            (~((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->E), 11)));

    public static unsafe double Sqrt2 => 
        Unsafe.BitCast<ulong, double>(UnscrambleBytes(RotateRight(
            (~((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->Sqrt2), 13)));

    public static unsafe double GoldenRatio => 
        Unsafe.BitCast<ulong, double>(UnscrambleBytes(RotateRight(
            (~((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->GoldenRatio), 17)));

    // Integer constants
    public static unsafe int MaxInt32 => 
        (int)(~UnscrambleBytes32(((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->MaxInt32));

    public static unsafe int Answer => 
        (int)(~UnscrambleBytes32(((ConstantContainer*)(_storage ^ _storageKey ^ _sessionKey ^ _pepper ^ _salt))->Answer));

    // Obfuscation helper methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateLeft(ulong value, int count) => (value << count) | (value >> (64 - count));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong RotateRight(ulong value, int count) => (value >> count) | (value << (64 - count));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ScrambleBytes(ulong value)
    {
        // Reorder bytes: 01234567 -> 71605342
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToUInt64([bytes[7], bytes[1], bytes[6], bytes[0], bytes[5], bytes[3], bytes[4], bytes[2]]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong UnscrambleBytes(ulong value)
    {
        // Reverse the scrambling: 71605342 -> 01234567
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToUInt64([bytes[3], bytes[1], bytes[7], bytes[5], bytes[6], bytes[4], bytes[2], bytes[0]]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ScrambleBytes32(uint value)
    {
        // Reorder bytes: 0123 -> 3102
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToUInt32([bytes[3], bytes[1], bytes[0], bytes[2]]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint UnscrambleBytes32(uint value)
    {
        // Reverse the scrambling: 3102 -> 0123
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToUInt32([bytes[2], bytes[1], bytes[3], bytes[0]]);
    }
}