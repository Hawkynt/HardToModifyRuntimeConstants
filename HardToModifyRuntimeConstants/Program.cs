using HardToModifyRuntimeConstants;
using System.Security.Cryptography;

Console.WriteLine("🔒 Hard-to-Modify Runtime Constants Demo");
Console.WriteLine("==========================================");

Console.WriteLine("\n📊 Basic Constants:");
Console.WriteLine($"Pi: {Constants.Pi:F15}");
Console.WriteLine($"E: {Constants.E:F15}");
Console.WriteLine($"√2: {Constants.Sqrt2:F15}");

Console.WriteLine("\n🚀 Enhanced Constants (with additional obfuscation):");
Console.WriteLine($"Pi: {EnhancedConstants.Pi:F15}");
Console.WriteLine($"E: {EnhancedConstants.E:F15}");
Console.WriteLine($"√2: {EnhancedConstants.Sqrt2:F15}");
Console.WriteLine($"Golden Ratio: {EnhancedConstants.GoldenRatio:F15}");
Console.WriteLine($"Max Int32: {EnhancedConstants.MaxInt32:N0}");
Console.WriteLine($"Answer to Everything: {EnhancedConstants.Answer}");

Console.WriteLine("\n🔐 Compile-Time Obfuscated Constants (MOST SECURE):");
try
{
    Console.WriteLine($"Pi: {SecureConstants.Pi:F15}");
    Console.WriteLine($"E: {SecureConstants.E:F15}");
    Console.WriteLine($"√2: {SecureConstants.Sqrt2:F15}");
    Console.WriteLine($"Golden Ratio: {SecureConstants.GoldenRatio:F15}");
    Console.WriteLine($"Max Int32: {SecureConstants.MaxInt32:N0}");
    Console.WriteLine($"Answer to Everything: {SecureConstants.Answer}");
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR in Level 3 constants: {ex.Message}");
}

Console.WriteLine("\n💰 High-Precision Decimal Constants:");
Console.WriteLine($"Pi (28 decimals): {DecimalConstants.PiDecimal}");
Console.WriteLine($"E (28 decimals): {DecimalConstants.EDecimal}");
Console.WriteLine($"One Percent: {DecimalConstants.OnePercent:P}");

Console.WriteLine("\n💎 Compile-Time Obfuscated Decimals (MOST SECURE):");
Console.WriteLine($"Pi (28 decimals): {SecureConstants.PiDecimal}");
Console.WriteLine($"E (28 decimals): {SecureConstants.EDecimal}");
Console.WriteLine($"One Percent: {SecureConstants.OnePercent:P}");

Console.WriteLine("\n🛡️ Level 4: Asymmetric Encryption (ULTIMATE SECURITY):");
Console.WriteLine(CryptoConstants.GetSecurityInfo());
Console.WriteLine("Attempting to access encrypted constants (will fail by design):");

try
{
    var pi = CryptoConstants.Pi;
    Console.WriteLine($"Pi: {pi}");
}
catch (CryptographicException ex)
{
    Console.WriteLine($"✅ SECURITY SUCCESS: {ex.Message}");
}

try
{
    var answer = CryptoConstants.Answer;
    Console.WriteLine($"Answer: {answer}");
}
catch (CryptographicException ex)
{
    Console.WriteLine($"✅ SECURITY SUCCESS: Access denied as expected");
}

Console.WriteLine("\n✅ All security levels demonstrated successfully!");
Console.WriteLine("🔒 Level 1: Basic runtime protection");
Console.WriteLine("🛡️ Level 2: Enhanced runtime obfuscation (VULNERABLE - original values visible)");
Console.WriteLine("🔐 Level 3: Compile-time obfuscation (RECOMMENDED)");
Console.WriteLine("🛡️ Level 4: Asymmetric encryption (ULTIMATE - values permanently sealed)");
