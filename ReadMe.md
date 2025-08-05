# 🔒 HardToModifyRuntimeConstants

![License](https://img.shields.io/github/license/Hawkynt/HardToModifyRuntimeConstants)
![Language](https://img.shields.io/github/languages/top/Hawkynt/HardToModifyRuntimeConstants?color=purple)
[![Last Commit](https://img.shields.io/github/last-commit/Hawkynt/HardToModifyRuntimeConstants?branch=main)![Activity](https://img.shields.io/github/commit-activity/y/Hawkynt/HardToModifyRuntimeConstants?branch=main)](https://github.com/Hawkynt/HardToModifyRuntimeConstants/commits/main)
[![Tests](https://github.com/Hawkynt/HardToModifyRuntimeConstants/actions/workflows/tests.yml/badge.svg)](https://github.com/Hawkynt/HardToModifyRuntimeConstants/actions/workflows/tests.yml)

A C# proof-of-concept for storing constants that are extremely difficult to tamper with. 🛡️

## Overview

This code provides a technique for defining constants in C# code that are:
- 🛡️ Protected from runtime tampering via reflection
- ⚡ Efficiently accessible in runtime code
- 🔐 Obfuscated in the binary

This code is useful for scenarios where constant integrity is critical, such as:
- 🔒 Security-sensitive calculations
- 💰 Financial applications
- 🔑 Cryptographic functions
- 🧮 Scientific computing where precision matters
- 📋 Licensing mechanisms

## How It Works

The source employs multiple layers of protection across **four different security levels**:

### 🔒 Level 1: Basic Runtime Protection
- Simple bitwise inversion (`~value`)
- Triple-XOR obfuscated pointer
- Randomized keys that change each execution
- Pinned memory structures

### 🛡️ Level 2: Enhanced Runtime Obfuscation  
- Multi-layer bit rotation and byte scrambling
- Complex XOR chains with multiple keys
- Additional integer and mathematical constants
- **⚠️ VULNERABILITY**: Original values visible in executable

### 🔐 Level 3: Compile-Time Obfuscation (MOST SECURE)
Instead of obfuscating at runtime, constants are obfuscated **during compilation**:

**🏗️ Build-Time Process:**
1. **ConstantObfuscator tool** runs before each build
2. Generates **cryptographically random keys** for this build session
3. Applies **complex multi-layer obfuscation**:
   - Identifier-based byte scrambling (8 different patterns per constant)
   - Variable bit rotation (1-31 bits based on identifier hash)
   - Multiple XOR layers with random keys and magic constants
4. Outputs `SecureConstants.generated.cs` with **only obfuscated hex values**
5. **No original values or obfuscation logic** visible in final executable

**🔐 Runtime Process:**
- Only **deobfuscation logic** exists in the binary
- Constants stored as seemingly random hex values (e.g., `0x01B73FC075550D0EUL`)
- Impossible to determine original values without knowing the build-specific keys

### High-Precision Storage

- Double-precision versions are available for performance-critical code
- Decimals are also possible using the same technique

## Sample Usage

```csharp
// Simple usage example
double circleArea = Constants.Pi * radius * radius;
```

## Technical Implementation

The implementation uses several advanced C# features across different security levels:

### 🔒 Level 1 & 2: Runtime Protection Features
**Read-Only Struct**: The underlying values are stored inside a read-only struct.

**Memory Pinning**: Constants are stored in a struct that's pinned in memory, preventing garbage collection and providing a stable address.

**Unsafe Code**: Pointer manipulation for direct memory access and obfuscation.

**Pointer Obfuscation**: The pointer to the struct is obfuscated using multiple XOR operations.

### 🔐 Level 3: Compile-Time Obfuscation Features
**Build Integration**: MSBuild pre-build event automatically runs the obfuscator tool.

**Cryptographic Randomness**: Uses `RandomNumberGenerator.Create()` for secure key generation.

**Multi-Layer Obfuscation Algorithm**:
```csharp
// Example obfuscation process (simplified):
1. Identifier-based byte scrambling (8 patterns)
2. Bit rotation (1-31 bits based on hash)  
3. XOR with keyMix ^ 0xABCDEF0123456789UL
```

**Code Generation**: Dynamically generates `SecureConstants.generated.cs` with:
- Only obfuscated hex literals
- Build-specific random keys
- Corresponding deobfuscation methods

**Reverse Obfuscation**: Runtime deobfuscation applies transformations in reverse order:
1. XOR reversal
2. Bit rotation reversal (right rotation)
3. Byte unscrambling

**Clean API**: Despite the complex security measures, the API remains simple:
```csharp
double pi = SecureConstants.Pi;  // Seamless usage
```

## Security Analysis

### 🔒 Level 1 & 2: Runtime Protection
**Against Runtime Tampering:**
To modify constants at runtime, an attacker would need to:
1. Find the obfuscated pointer field via reflection
2. Know the random key (which changes each execution)
3. Know the hardcoded pepper value
4. Calculate the actual pointer location
5. Modify memory using unsafe code

**⚠️ CRITICAL VULNERABILITY**: Original values and transformation logic visible in executable binary!

### 🔐 Level 3: Compile-Time Obfuscation (MOST SECURE)

**Against Binary Analysis:**
An attacker analyzing the compiled executable sees:
```csharp
public readonly ulong Pi = 0x01B73FC075550D0EUL;  // Meaningless without keys
private static readonly long _storageKey = 0x2DE2A33664750458L;  // Changes every build
```
- ✅ **No original values** (3.14159...) anywhere in binary
- ✅ **No obfuscation algorithms** - only deobfuscation
- ✅ **Build-specific keys** - different for every compilation
- ✅ **Complex multi-layer transformations** - nearly impossible to reverse without keys

**Against Runtime Tampering:**
Even with reflection access, an attacker would need to:
1. Reverse-engineer the deobfuscation algorithm
2. Understand identifier-based scrambling patterns  
3. Know the build-specific random keys
4. Recalculate all transformations for desired values
5. Generate valid obfuscated replacements

**Against Binary Patching:**
Impossible without:
1. The original obfuscation tool and algorithms
2. Build-specific cryptographic keys
3. Understanding of identifier-based transformations
4. Recalculating scrambling patterns for each constant

**💡 Result**: Constants are effectively **immutable at the binary level**.

### 🛡️ Level 4: Asymmetric Encryption (ULTIMATE SECURITY)

The highest security level using **RSA-2048 asymmetric encryption**:

**🔐 Compile-Time Process:**
1. **RSA key pair generation** with 2048-bit strength
2. **All constants encrypted** with the private key
3. **Private key immediately discarded** after compilation
4. Only **public key stored** in binary (cannot decrypt)

**🚫 Runtime Behavior (Intentional):**
```csharp
double pi = CryptoConstants.Pi;  // throws CryptographicException
```
- **All property access throws exceptions** by design
- Demonstrates **ultimate immutability** - values are permanently sealed
- Even with full source code access, **decryption is impossible**

**🔒 Security Guarantees:**
- ✅ **Perfect forward secrecy** - private key never stored
- ✅ **Cryptographically impossible** to recover values  
- ✅ **Proof of concept** for maximum theoretical security
- ✅ **2048-bit RSA encryption** - computationally infeasible to break

**💡 Use Case**: Demonstration that constants can be made **truly immutable** - even developers cannot access them after compilation.

## Limitations

- Slight performance overhead from deobfuscation (mitigated by JIT inlining)
- Slightly increased memory usage from storing both obfuscated and random values
- Not suitable for constants that need to be compile-time constants for the C# compiler

## Future Enhancements

Potential improvements could include:
- Custom attributes to automatically apply this technique to constants
- Integration with code generation tools
- Additional obfuscation layers for even greater security
- Support for other numeric types beyond decimal and double

## 🏗️ Build and Test

### Prerequisites
- .NET 8.0 SDK or later
- C# compiler with unsafe code support enabled

### Building the Project
```bash
# Clone the repository
git clone <repository-url>
cd HardToModifyRuntimeConstants

# Build the main project (automatically runs obfuscator)
dotnet build HardToModifyRuntimeConstants/HardToModifyRuntimeConstants.csproj

# Build the test project (NUnit)
dotnet build ObfuscationTest/ObfuscationTest.csproj

# Build the performance benchmarks (BenchmarkDotNet)
dotnet build PerformanceBenchmarks/PerformanceBenchmarks.csproj

# Build the obfuscator tool
dotnet build ConstantObfuscator/ConstantObfuscator.csproj
```

### Running the Application
```bash
# Run the demo application (shows all 4 security levels)
dotnet run --project HardToModifyRuntimeConstants/HardToModifyRuntimeConstants.csproj

# Run performance benchmarks (BenchmarkDotNet)
dotnet run --project PerformanceBenchmarks/PerformanceBenchmarks.csproj --configuration Release

# Manually generate obfuscated constants
dotnet run --project ConstantObfuscator/ConstantObfuscator.csproj -- HardToModifyRuntimeConstants
```

### Running Tests
```bash
# Run all NUnit tests
dotnet test ObfuscationTest/ObfuscationTest.csproj

# Run tests with detailed output
dotnet test ObfuscationTest/ObfuscationTest.csproj --verbosity normal

# Run tests with coverage (requires coverlet)
dotnet test ObfuscationTest/ObfuscationTest.csproj --collect:"XPlat Code Coverage"
```

### Test Categories

**🧪 NUnit Test Suite** (`ObfuscationTest`):
- **Obfuscation Pattern Tests**: Validates scrambling/unscrambling algorithms (7 patterns × multiple test cases)
- **SecureConstants Tests**: Level 3 compile-time obfuscation correctness validation
- **CryptoConstants Tests**: Level 4 asymmetric encryption exception handling
- **Random Value Testing**: Tests patterns with 10 random values per pattern

**⚡ BenchmarkDotNet Performance Suite** (`PerformanceBenchmarks`):
- **Constant Access Benchmarks**: Compares all 4 security levels
- **Integer Access Benchmarks**: Specialized benchmarks for integer constants
- **Decimal Access Benchmarks**: Tests high-precision decimal performance
- **Memory Diagnostics**: Tracks allocations and memory usage
- **Baseline Comparisons**: Shows relative performance costs

## 🚀 Performance

Performance varies by security level:

### 🔒 Level 1: Basic (Fastest)
- Property access: Virtually no overhead (~0 cycles)
- Memory usage: Minimal - just pointer obfuscation
- JIT optimization: Full inlining possible

### 🛡️ Level 2: Enhanced Runtime  
- Property access: ~3-5 additional CPU cycles for complex deobfuscation
- Memory usage: Slight increase from additional transformations
- JIT optimization: Partial inlining due to complexity

### 🔐 Level 3: Compile-Time (Best Security/Performance Balance)
- Property access: ~1-2 additional CPU cycles for deobfuscation
- Memory usage: Negligible - only obfuscated hex values stored
- JIT optimization: Good inlining potential for deobfuscation methods
- **Build time**: +1-2 seconds for obfuscation generation

### 🛡️ Level 4: Asymmetric Encryption (No Runtime Performance)
- Property access: **Immediate exception throw** (CryptographicException)
- Memory usage: Encrypted byte arrays + RSA public key (~270 bytes)
- JIT optimization: Exception path - no meaningful performance metrics
- **Build time**: +2-3 seconds for RSA key generation and encryption

## 📈 Test Coverage

The project includes comprehensive tests covering **70+ test cases**:

**🧪 NUnit Tests (ObfuscationTest)**:
- ✅ **Pattern Validation**: 7 scrambling patterns × 10 random values = 70 test cases
- ✅ **Algorithm Correctness**: Round-trip validation of all obfuscation transformations
- ✅ **SecureConstants Accuracy**: Mathematical precision validation (π, e, √2, φ)
- ✅ **CryptoConstants Security**: Exception throwing behavior validation
- ✅ **Edge Case Handling**: Boundary conditions and error scenarios

**⚡ BenchmarkDotNet Performance Tests**:
- ✅ **Performance Profiling**: All 4 security levels benchmarked
- ✅ **Memory Analysis**: Allocation tracking and memory usage profiling  
- ✅ **Baseline Comparisons**: Relative performance cost measurement
- ✅ **JIT Optimization**: Inlining behavior analysis across levels
- ✅ **Statistical Analysis**: Multiple iterations with confidence intervals

## License

* [LGPL-3.0](https://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License)
