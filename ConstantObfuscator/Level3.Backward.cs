// MUST NOT HAVE USINGS!

namespace HardToModifyRuntimeConstants
{

    partial class SecureConstants
    {
        private static uint Hash(string name) => System.Text.Encoding.Unicode.GetBytes(name).Aggregate(0U, (c, n) => ((c << 7) | (c >> 25)) ^ n);

        internal static unsafe ulong ReverseComplexObfuscation(ulong obfuscated, long key, string identifier)
        {
            var identifierHash = Hash(identifier);
            ulong keyMix = (ulong)(key ^ identifierHash);

            // Step 3 (reverse): XOR with keys and magic constant
            ulong rotated = obfuscated ^ keyMix ^ 0xABCDEF0123456789UL;

            // Step 2 (reverse): Bit rotation - RIGHT rotation to undo LEFT rotation
            int rotation = (int)(identifierHash % 31) + 1; // 1-31 bit rotation
            ulong scrambled = (rotated >> rotation) | (rotated << (64 - rotation));

            // Step 1 (reverse): Byte unscrambling - inverse of forward scrambling
            byte pattern = (byte)(identifierHash % 8);
            ulong value;
            
            byte* srcPtr = (byte*)&scrambled;
            byte* destPtr = (byte*)&value;

            switch (pattern)
            {
                case 0:
                    (destPtr[0], destPtr[1], destPtr[2], destPtr[3], destPtr[4], destPtr[5], destPtr[6], destPtr[7])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 1:
                    (destPtr[3], destPtr[1], destPtr[7], destPtr[0], destPtr[4], destPtr[6], destPtr[2], destPtr[5])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 2:
                    (destPtr[5], destPtr[2], destPtr[0], destPtr[6], destPtr[3], destPtr[7], destPtr[1], destPtr[4])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 3:
                    (destPtr[6], destPtr[3], destPtr[1], destPtr[5], destPtr[7], destPtr[0], destPtr[4], destPtr[2])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 4:
                    (destPtr[2], destPtr[6], destPtr[4], destPtr[1], destPtr[0], destPtr[5], destPtr[7], destPtr[3])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 5:
                    (destPtr[1], destPtr[7], destPtr[5], destPtr[3], destPtr[6], destPtr[2], destPtr[0], destPtr[4])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 6:
                    (destPtr[4], destPtr[0], destPtr[6], destPtr[2], destPtr[1], destPtr[3], destPtr[5], destPtr[7])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
                case 7:
                    (destPtr[7], destPtr[4], destPtr[2], destPtr[6], destPtr[5], destPtr[1], destPtr[3], destPtr[0])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3], srcPtr[4], srcPtr[5], srcPtr[6], srcPtr[7]);
                    break;
            }
            
            return value;
        }

        internal static unsafe uint ReverseComplexObfuscation32(uint obfuscated, long key, string identifier)
        {
            var identifierHash = Hash(identifier);
            uint keyMix = (uint)(key ^ identifierHash);

            // Step 3 (reverse): XOR
            uint rotated = obfuscated ^ keyMix ^ 0x12345678U;

            // Step 2 (reverse): Bit rotation - RIGHT rotation to undo LEFT
            int rotation = (int)(identifierHash % 15) + 1; // 1-15 bit rotation
            uint scrambled = (rotated >> rotation) | (rotated << (32 - rotation));

            // Step 1 (reverse): Byte unscrambling
            byte pattern = (byte)(identifierHash % 4);
            uint value;

            byte* srcPtr = (byte*)&scrambled;
            byte* destPtr = (byte*)&value;
            switch (pattern)
            {
                case 0:
                    (destPtr[0], destPtr[1], destPtr[2], destPtr[3])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3]);
                    break;
                case 1:
                    (destPtr[3], destPtr[1], destPtr[0], destPtr[2])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3]);
                    break;
                case 2:
                    (destPtr[2], destPtr[0], destPtr[3], destPtr[1])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3]);
                    break;
                case 3:
                    (destPtr[1], destPtr[2], destPtr[3], destPtr[0])
                  = (srcPtr[0], srcPtr[1], srcPtr[2], srcPtr[3]);
                    break;
            }

            return value;
        }

    }
}