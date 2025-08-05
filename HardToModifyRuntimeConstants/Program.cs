using HardToModifyRuntimeConstants;
using System.Security.Cryptography;

Console.WriteLine("🔒 Hard-to-Modify Runtime Constants Demo");
Console.WriteLine("==========================================");

// Level 1
Console.WriteLine("\n📊 Basic Constants:");
Console.WriteLine($"Pi: {Constants.Pi:F15}");
Console.WriteLine($"E: {Constants.E:F15}");
Console.WriteLine($"√2: {Constants.Sqrt2:F15}");

// Level 2
Console.WriteLine("\n🚀 Enhanced Constants (with additional obfuscation):");
Console.WriteLine($"Pi: {EnhancedDoubleConstants.Pi:F15}");
Console.WriteLine($"E: {EnhancedDoubleConstants.E:F15}");
Console.WriteLine($"√2: {EnhancedDoubleConstants.Sqrt2:F15}");
Console.WriteLine($"Golden Ratio: {EnhancedDoubleConstants.GoldenRatio:F15}");
Console.WriteLine($"Max Int32: {EnhancedDoubleConstants.MaxInt32:N0}");
Console.WriteLine($"Answer to Everything: {EnhancedDoubleConstants.Answer}");
Console.WriteLine("\n💰 High-Precision Decimal Constants:");
Console.WriteLine($"Pi (28 decimals): {EnhancedDecimalConstants.PiDecimal}");
Console.WriteLine($"E (28 decimals): {EnhancedDecimalConstants.EDecimal}");
Console.WriteLine($"One Percent: {EnhancedDecimalConstants.OnePercent:P}");

// Level 3
Console.WriteLine("\n🔐 Compile-Time Obfuscated Constants:");
try
{
    Console.WriteLine($"Pi: {SecureConstants.Pi:F15}");
    Console.WriteLine($"E: {SecureConstants.E:F15}");
    Console.WriteLine($"√2: {SecureConstants.Sqrt2:F15}");
    Console.WriteLine($"Golden Ratio: {SecureConstants.GoldenRatio:F15}");
    Console.WriteLine($"Max Int32: {SecureConstants.MaxInt32:N0}");
    Console.WriteLine($"Answer to Everything: {SecureConstants.Answer}");
    Console.WriteLine("\n💎 Compile-Time Obfuscated Decimals:");
    Console.WriteLine($"Pi (28 decimals): {SecureConstants.PiDecimal}");
    Console.WriteLine($"E (28 decimals): {SecureConstants.EDecimal}");
    Console.WriteLine($"One Percent: {SecureConstants.OnePercent:P}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ERROR in Level 3 constants: {ex.Message}");
}

// Level 4
Console.WriteLine("\n🛡️ Level 4: One-Way Decryption:");
Console.WriteLine(CryptoConstants.GetSecurityInfo());
Console.WriteLine("Attempting to access encrypted constants (will be decrypted at runtime):");

try
{
    Console.WriteLine($"Pi: {CryptoConstants.Pi:F15}");
    Console.WriteLine($"E: {CryptoConstants.E:F15}");
    Console.WriteLine($"√2: {CryptoConstants.Sqrt2:F15}");
    Console.WriteLine($"Golden Ratio: {CryptoConstants.GoldenRatio:F15}");
    Console.WriteLine($"Max Int32: {CryptoConstants.MaxInt32:N0}");
    Console.WriteLine($"Answer to Everything: {CryptoConstants.Answer}");
    Console.WriteLine($"Pi (Decimal): {CryptoConstants.PiDecimal}");
    Console.WriteLine($"E (Decimal): {CryptoConstants.EDecimal}");
    Console.WriteLine($"One Percent: {CryptoConstants.OnePercent:P}");
}
catch (CryptographicException ex)
{
    Console.WriteLine($"❌ ERROR: Decryption failed! {ex.Message}");
}

Console.WriteLine("\n✅ All security levels demonstrated successfully!");
Console.WriteLine("🔒 Level 1: Basic runtime protection");
Console.WriteLine("🛡️ Level 2: Enhanced runtime obfuscation (VULNERABLE - original values visible)");
Console.WriteLine("🔐 Level 3: Compile-time obfuscation (RECOMMENDED)");
Console.WriteLine("🛡️ Level 4: Asymmetric encryption (decryptable at runtime)");
