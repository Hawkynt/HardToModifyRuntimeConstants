using System;
using NUnit.Framework;
using HardToModifyRuntimeConstants;

namespace ObfuscationTest;

[TestFixture]
public unsafe class ObfuscationPatternTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    public void TestScramblePatternRoundTrip(int pattern)
    {
        // Test with a known value
        ulong testValue = 0x0123456789ABCDEFUL;
        
        // Apply forward scrambling
        ulong scrambled = ApplyScrambling(testValue, pattern);
        
        // Apply reverse scrambling  
        ulong unscrambled = ApplyReverseScrambling(scrambled, pattern);
        
        Assert.That(unscrambled, Is.EqualTo(testValue), 
            $"Pattern {pattern} failed: Original=0x{testValue:X16}, Scrambled=0x{scrambled:X16}, Unscrambled=0x{unscrambled:X16}");
    }

    [Test]
    public void TestAllPatternsWithRandomValues()
    {
        var random = new Random(42); // Fixed seed for reproducible tests
        
        for (int pattern = 1; pattern <= 7; pattern++)
        {
            for (int i = 0; i < 10; i++)
            {
                // Generate random test value
                byte[] bytes = new byte[8];
                random.NextBytes(bytes);
                ulong testValue = BitConverter.ToUInt64(bytes);
                
                ulong scrambled = ApplyScrambling(testValue, pattern);
                ulong unscrambled = ApplyReverseScrambling(scrambled, pattern);
                
                Assert.That(unscrambled, Is.EqualTo(testValue), 
                    $"Pattern {pattern} failed with random value 0x{testValue:X16}");
            }
        }
    }

    [TestFixture]
    public class SecureConstantsTests
    {
        [Test]
        public void TestSecureConstantsValues()
        {
            // Test that SecureConstants produce expected mathematical values
            Assert.That(SecureConstants.Pi, Is.EqualTo(Math.PI).Within(1e-15), "Pi value should match Math.PI");
            Assert.That(SecureConstants.E, Is.EqualTo(Math.E).Within(1e-15), "E value should match Math.E");
            Assert.That(SecureConstants.Sqrt2, Is.EqualTo(Math.Sqrt(2)).Within(1e-15), "Sqrt2 value should match Math.Sqrt(2)");
            Assert.That(SecureConstants.GoldenRatio, Is.EqualTo((1 + Math.Sqrt(5)) / 2).Within(1e-15), "Golden ratio should be correct");
            Assert.That(SecureConstants.MaxInt32, Is.EqualTo(int.MaxValue), "MaxInt32 should equal int.MaxValue");
            Assert.That(SecureConstants.Answer, Is.EqualTo(42), "Answer should be 42");
        }

        [Test]
        public void TestCryptoConstantsThrowsException()
        {
            // Test that Level 4 constants properly throw exceptions
            Assert.Throws<System.Security.Cryptography.CryptographicException>(() => { var _ = CryptoConstants.Pi; });
            Assert.Throws<System.Security.Cryptography.CryptographicException>(() => { var _ = CryptoConstants.Answer; });
        }

        [Test]
        public void TestCryptoConstantsSecurityInfo()
        {
            // Test that security info is returned
            string info = CryptoConstants.GetSecurityInfo();
            Assert.That(info, Does.Contain("RSA-2048"));
            Assert.That(info, Does.Contain("Private key existed only during compilation"));
        }
    }
    
    static unsafe void ShowByteComparison(ulong original, ulong recovered)
    {
        byte* origPtr = (byte*)&original;
        byte* recPtr = (byte*)&recovered;
        
        Console.Write("Original bytes: ");
        for (int i = 0; i < 8; i++) Console.Write($"{origPtr[i]:X2} ");
        Console.WriteLine();
        
        Console.Write("Recovered bytes:");
        for (int i = 0; i < 8; i++) Console.Write($"{recPtr[i]:X2} ");
        Console.WriteLine();
    }
    
    static unsafe ulong ApplyScrambling(ulong value, int pattern)
    {
        ulong scrambled;
        byte* srcPtr = (byte*)&value;
        byte* destPtr = (byte*)&scrambled;
        
        switch (pattern)
        {
            case 1: // scramble: 0,1,2,3,4,5,6,7 -> 3,1,7,0,4,6,2,5
                destPtr[0] = srcPtr[3]; destPtr[1] = srcPtr[1]; destPtr[2] = srcPtr[7]; destPtr[3] = srcPtr[0];
                destPtr[4] = srcPtr[4]; destPtr[5] = srcPtr[6]; destPtr[6] = srcPtr[2]; destPtr[7] = srcPtr[5];
                break;
            case 2: // scramble: 0,1,2,3,4,5,6,7 -> 5,2,0,6,3,7,1,4
                destPtr[0] = srcPtr[5]; destPtr[1] = srcPtr[2]; destPtr[2] = srcPtr[0]; destPtr[3] = srcPtr[6];
                destPtr[4] = srcPtr[3]; destPtr[5] = srcPtr[7]; destPtr[6] = srcPtr[1]; destPtr[7] = srcPtr[4];
                break;
            case 3: // scramble: 0,1,2,3,4,5,6,7 -> 6,3,1,5,7,0,4,2  
                destPtr[0] = srcPtr[6]; destPtr[1] = srcPtr[3]; destPtr[2] = srcPtr[1]; destPtr[3] = srcPtr[5];
                destPtr[4] = srcPtr[7]; destPtr[5] = srcPtr[0]; destPtr[6] = srcPtr[4]; destPtr[7] = srcPtr[2];
                break;
            case 4: // scramble: 0,1,2,3,4,5,6,7 -> 2,6,4,1,0,5,7,3
                destPtr[0] = srcPtr[2]; destPtr[1] = srcPtr[6]; destPtr[2] = srcPtr[4]; destPtr[3] = srcPtr[1];
                destPtr[4] = srcPtr[0]; destPtr[5] = srcPtr[5]; destPtr[6] = srcPtr[7]; destPtr[7] = srcPtr[3];
                break;
            case 5: // scramble: 0,1,2,3,4,5,6,7 -> 1,7,5,3,6,2,0,4
                destPtr[0] = srcPtr[1]; destPtr[1] = srcPtr[7]; destPtr[2] = srcPtr[5]; destPtr[3] = srcPtr[3];
                destPtr[4] = srcPtr[6]; destPtr[5] = srcPtr[2]; destPtr[6] = srcPtr[0]; destPtr[7] = srcPtr[4];
                break;
            case 6: // scramble: 0,1,2,3,4,5,6,7 -> 4,0,6,2,1,3,5,7
                destPtr[0] = srcPtr[4]; destPtr[1] = srcPtr[0]; destPtr[2] = srcPtr[6]; destPtr[3] = srcPtr[2];
                destPtr[4] = srcPtr[1]; destPtr[5] = srcPtr[3]; destPtr[6] = srcPtr[5]; destPtr[7] = srcPtr[7];
                break;
            case 7: // scramble: 0,1,2,3,4,5,6,7 -> 7,5,2,6,1,4,3,0
                destPtr[0] = srcPtr[7]; destPtr[1] = srcPtr[4]; destPtr[2] = srcPtr[2]; destPtr[3] = srcPtr[6];
                destPtr[4] = srcPtr[5]; destPtr[5] = srcPtr[1]; destPtr[6] = srcPtr[3]; destPtr[7] = srcPtr[0];
                break;
            default:
                scrambled = value;
                break;
        }
        return scrambled;
    }
    
    static unsafe ulong ApplyReverseScrambling(ulong scrambled, int pattern)
    {
        ulong result;
        byte* ptr = (byte*)&scrambled;
        byte* resultPtr = (byte*)&result;
        
        // To create the correct reverse, I need to map each destination back to source
        switch (pattern)
        {
            case 1: // Forward was: 0->3, 1->1, 2->7, 3->0, 4->4, 5->6, 6->2, 7->5
                    // Reverse must be: 0<-3, 1<-1, 2<-6, 3<-0, 4<-4, 5<-7, 6<-5, 7<-2
                resultPtr[0] = ptr[3]; resultPtr[1] = ptr[1]; resultPtr[2] = ptr[6]; resultPtr[3] = ptr[0];
                resultPtr[4] = ptr[4]; resultPtr[5] = ptr[7]; resultPtr[6] = ptr[5]; resultPtr[7] = ptr[2];
                break;
            case 2: // Forward was: 0->5, 1->2, 2->0, 3->6, 4->3, 5->7, 6->1, 7->4  
                    // Reverse must be: 0<-2, 1<-6, 2<-1, 3<-4, 4<-7, 5<-0, 6<-3, 7<-5
                resultPtr[0] = ptr[2]; resultPtr[1] = ptr[6]; resultPtr[2] = ptr[1]; resultPtr[3] = ptr[4];
                resultPtr[4] = ptr[7]; resultPtr[5] = ptr[0]; resultPtr[6] = ptr[3]; resultPtr[7] = ptr[5];
                break;
            case 3: // Forward was: 0->6, 1->3, 2->1, 3->5, 4->7, 5->0, 6->4, 7->2
                    // Reverse must be: 0<-5, 1<-2, 2<-7, 3<-1, 4<-6, 5<-3, 6<-0, 7<-4  
                resultPtr[0] = ptr[5]; resultPtr[1] = ptr[2]; resultPtr[2] = ptr[7]; resultPtr[3] = ptr[1];
                resultPtr[4] = ptr[6]; resultPtr[5] = ptr[3]; resultPtr[6] = ptr[0]; resultPtr[7] = ptr[4];
                break;
            case 4: // Forward was: 0->2, 1->6, 2->4, 3->1, 4->0, 5->5, 6->7, 7->3
                    // Reverse must be: 0<-4, 1<-3, 2<-0, 3<-7, 4<-2, 5<-5, 6<-1, 7<-6
                resultPtr[0] = ptr[4]; resultPtr[1] = ptr[3]; resultPtr[2] = ptr[0]; resultPtr[3] = ptr[7];
                resultPtr[4] = ptr[2]; resultPtr[5] = ptr[5]; resultPtr[6] = ptr[1]; resultPtr[7] = ptr[6];
                break;
            case 5: // Forward was: 0->1, 1->7, 2->5, 3->3, 4->6, 5->2, 6->0, 7->4
                    // Reverse must be: 0<-6, 1<-0, 2<-5, 3<-3, 4<-7, 5<-2, 6<-4, 7<-1
                resultPtr[0] = ptr[6]; resultPtr[1] = ptr[0]; resultPtr[2] = ptr[5]; resultPtr[3] = ptr[3];
                resultPtr[4] = ptr[7]; resultPtr[5] = ptr[2]; resultPtr[6] = ptr[4]; resultPtr[7] = ptr[1];
                break;
            case 6: // Forward was: 0->4, 1->0, 2->6, 3->2, 4->1, 5->3, 6->5, 7->7
                    // Reverse must be: 0<-1, 1<-4, 2<-3, 3<-5, 4<-0, 5<-6, 6<-2, 7<-7
                resultPtr[0] = ptr[1]; resultPtr[1] = ptr[4]; resultPtr[2] = ptr[3]; resultPtr[3] = ptr[5];
                resultPtr[4] = ptr[0]; resultPtr[5] = ptr[6]; resultPtr[6] = ptr[2]; resultPtr[7] = ptr[7];
                break;
            case 7: // Forward was: 0->7, 1->4, 2->2, 3->6, 4->5, 5->1, 6->3, 7->0
                    // Reverse must be: 0<-7, 1<-5, 2<-2, 3<-6, 4<-1, 5<-4, 6<-3, 7<-0
                resultPtr[0] = ptr[7]; resultPtr[1] = ptr[5]; resultPtr[2] = ptr[2]; resultPtr[3] = ptr[6];
                resultPtr[4] = ptr[1]; resultPtr[5] = ptr[4]; resultPtr[6] = ptr[3]; resultPtr[7] = ptr[0];
                break;
            default:
                result = scrambled;
                break;
        }
        return result;
    }
}