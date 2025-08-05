internal static unsafe ulong ReverseComplexObfuscation(ulong rotated, long key, string identifier)
{
    uint identifierHash = (uint)identifier.GetHashCode();
    ulong keyMix = (ulong)(key ^ identifierHash);

    // Step 1: Byte scrambling based on identifier hash using unsafe pointers
    byte pattern = (byte)(identifierHash % 8);
    ulong value;

    // Step 3: XOR with keys and magic constant
    rotated ^= keyMix ^ 0xABCDEF0123456789UL;

    // Step 2: Bit rotation based on identifier
    int rotation = (int)(identifierHash % 31) + 1; // 1-31 bit rotation
    ulong scrambled = (rotated >> rotation) | (rotated << (64 - rotation));

    byte* srcPtr = (byte*)&value;
    byte* destPtr = (byte*)&scrambled;

    switch (pattern)
    {
        case 0:
            (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 1:
            (srcPtr[3], srcPtr[1], srcPtr[7], srcPtr[0], srcPtr[4], srcPtr[6], srcPtr[2], srcPtr[5])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 2:
            (srcPtr[5], srcPtr[2], srcPtr[0], srcPtr[6], srcPtr[3], srcPtr[7], srcPtr[1], srcPtr[4])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 3:
            (srcPtr[6], srcPtr[3], srcPtr[1], srcPtr[5], srcPtr[7], srcPtr[0], srcPtr[4], srcPtr[2])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 4:
            (srcPtr[2], srcPtr[6], srcPtr[4], srcPtr[1], srcPtr[0], srcPtr[5], srcPtr[7], srcPtr[3])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 5:
            (srcPtr[1], srcPtr[7], srcPtr[5], srcPtr[3], srcPtr[6], srcPtr[2], srcPtr[0], srcPtr[4])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 6:
            (srcPtr[4], srcPtr[0], srcPtr[6], srcPtr[2], srcPtr[1], srcPtr[3], srcPtr[5], srcPtr[7])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
        case 7:
            (srcPtr[7], srcPtr[4], srcPtr[2], srcPtr[6], srcPtr[5], srcPtr[1], srcPtr[3], srcPtr[0])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7]);
            break;
    }
    
    return value;
}

internal static unsafe uint ReverseComplexObfuscation32(uint rotated, long key, string identifier)
{
    uint identifierHash = (uint)identifier.GetHashCode();
    uint keyMix = (uint)(key ^ identifierHash);

    rotated ^= keyMix ^ 0x12345678U

    // Bit rotation
    int rotation = (int)(identifierHash % 15) + 1; // 1-15 bit rotation
    uint scrambled = (rotated >> rotation) | (rotated << (32 - rotation));

    // Byte scrambling for 32-bit values using unsafe pointers
    byte pattern = (byte)(identifierHash % 4);
    uint value;

    byte* srcPtr = (byte*)&value;
    byte* destPtr = (byte*)&scrambled;
    switch (pattern)
    {
        case 0:
            (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3]);
            break;
        case 1:
            (srcPtr[3], srcPtr[1], srcPtr[0], srcPtr[2])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3]);
            break;
        case 2:
            (srcPtr[2], srcPtr[0], srcPtr[3], srcPtr[1])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3]);
            break;
        case 3:
            (srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[0])
          = (destPtr[0], destPtr[1], destPtr[2], destPtr[3]);
            break;
    }

    return value;
}