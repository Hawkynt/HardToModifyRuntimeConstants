using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstantObfuscator;

internal class Level3
{

    private static uint Hash(string name) => Encoding.Unicode.GetBytes(name).Aggregate(0U, (c,n)=>((c << 7) | (c >> 25)) ^ n );

    internal static unsafe ulong ApplyComplexObfuscation(ulong value, long key, string identifier)
    {
        var identifierHash = Hash(identifier);
        ulong keyMix = (ulong)(key ^ identifierHash);

        // Step 1: Byte scrambling based on identifier hash using unsafe pointers
        byte pattern = (byte)(identifierHash % 8);
        ulong scrambled;
        
        byte* srcPtr = (byte*)&value;
        byte* destPtr = (byte*)&scrambled;

        (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]) = pattern switch
        {
            1 => (srcPtr[3], srcPtr[1], srcPtr[7], srcPtr[0], srcPtr[4], srcPtr[6], srcPtr[2], srcPtr[5]),
            2 => (srcPtr[5], srcPtr[2], srcPtr[0], srcPtr[6], srcPtr[3], srcPtr[7], srcPtr[1], srcPtr[4]),
            3 => (srcPtr[6], srcPtr[3], srcPtr[1], srcPtr[5], srcPtr[7], srcPtr[0], srcPtr[4], srcPtr[2]),
            4 => (srcPtr[2], srcPtr[6], srcPtr[4], srcPtr[1], srcPtr[0], srcPtr[5], srcPtr[7], srcPtr[3]),
            5 => (srcPtr[1], srcPtr[7], srcPtr[5], srcPtr[3], srcPtr[6], srcPtr[2], srcPtr[0], srcPtr[4]),
            6 => (srcPtr[4], srcPtr[0], srcPtr[6], srcPtr[2], srcPtr[1], srcPtr[3], srcPtr[5], srcPtr[7]),
            7 => (srcPtr[7], srcPtr[4], srcPtr[2], srcPtr[6], srcPtr[5], srcPtr[1], srcPtr[3], srcPtr[0]),
            _ => (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7])
        };
        
        // Step 2: Bit rotation based on identifier
        int rotation = (int)(identifierHash % 31) + 1; // 1-31 bit rotation
        ulong rotated = (scrambled << rotation) | (scrambled >> (64 - rotation));

        // Step 3: XOR with keys and magic constant
        return rotated ^ keyMix ^ 0xABCDEF0123456789UL;
    }

    internal static unsafe uint ApplyComplexObfuscation32(uint value, long key, string identifier)
    {
        var identifierHash = Hash(identifier);
        uint keyMix = (uint)(key ^ identifierHash);

        // Byte scrambling for 32-bit values using unsafe pointers
        byte pattern = (byte)(identifierHash % 4);
        uint scrambled;

        byte* srcPtr = (byte*)&value;
        byte* destPtr = (byte*)&scrambled;
        (destPtr[0], destPtr[1], destPtr[2], destPtr[3]) = pattern switch
        {
            1 => (srcPtr[3], srcPtr[1], srcPtr[0], srcPtr[2]),
            2 => (srcPtr[2], srcPtr[0], srcPtr[3], srcPtr[1]),
            3 => (srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[0]),
            _ => (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3])
        };

        // Bit rotation
        int rotation = (int)(identifierHash % 15) + 1; // 1-15 bit rotation
        uint rotated = (scrambled << rotation) | (scrambled >> (32 - rotation));

        return rotated ^ keyMix ^ 0x12345678U;
    }
}
