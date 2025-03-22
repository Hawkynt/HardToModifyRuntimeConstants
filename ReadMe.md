# HardToModifyRuntimeConstants

A C# proof-of-concept for storing constants that are extremely difficult to tamper with.

## Overview

This code provides a technique for defining constants in C# code that are:
- Protected from runtime tampering via reflection
- Efficiently accessible in runtime code
- Obfuscated in the binary

This code is useful for scenarios where constant integrity is critical, such as:
- Security-sensitive calculations
- Financial applications
- Cryptographic functions
- Scientific computing where precision matters
- Licensing mechanisms

## How It Works

The source employs multiple layers of protection:

### Obfuscated Values

Values can be obfuscated at runtime if performance impact is not a concern
- You could XOR the values
- You could rotate bits
- You could re-order the individual bytes
- You could re-combine bits from different bytes (P-Boxes)
- You could apply Substition (S-Boxes)

### Runtime Protection

Instead of directly exposing constants, they're:
- Stored in a pinned `readonly struct` in memory
- Referenced via a triple-XOR obfuscated pointer
- Protected with a randomized key that changes each program execution
- Combined with a hardcoded "pepper" value

### High-Precision Storage

- Double-precision versions are available for performance-critical code
- Decimals are also possible using the same technique

## Sample Usage

```csharp
// Simple usage example
double circleArea = Constants.Pi * radius * radius;
```

## Technical Implementation

The implementation uses several advanced C# features:

**Read-Only Struct**: The underlying values are stored inside a read-only struct.

**Heap and Stack**: While the struct itself is allocated on the local stack of a static constructor, it is boxed somewhere to the heap.

**Memory Pinning**: Constants are stored in a struct that's pinned in memory, preventing garbage collection and providing a stable address.

**Unsafe Code**: To mess with pointers we'll employ some unsafe code. But don't worry there's nothing to fear as we will only use one-liners.

**Pointer Obfuscation**: The pointer to the struct is obfuscated using triple-XOR with:
   - A random key generated at runtime (`_storageKey`)
   - A hardcoded "pepper" value (`_pepper`) to make it harder to swap the pointer via reflection and utilizing the random key
   - This makes it extremely difficult to locate the constants in memory
   - This makes it even more difficult to tamper them during runtime

**Read-Only static fields**: The modified pointer is stored in a static read-only field to make modification even harder and allow the JIT to inline the value in compiled methods. That opens the possibility, that even IF the pointers gets somehow modified, its original value may be already backed into the machine code of the property getter thus making the modification meaningless for the attacker.

**Clean API**: Despite the complex security measures, the API remains simple and straightforward for developers.

## Security Analysis

### Against Runtime Tampering

To modify constants at runtime, an attacker would need to:
1. Find the obfuscated pointer field via reflection
2. Know the random key (which changes each execution) or also find that via reflection
3. Know the hardcoded pepper value
4. Calculate the actual pointer location
5. Initiate another instance of the internal struct or modify bits using unsafe code
6. Swap the pointers

### Against Binary Patching

To modify constants via hex editing, an attacker would need to:
1. Locate the obfuscated constant values in the binary
2. Understand the bit transformation
3. Calculate what values to patch to achieve desired constants
4. Avoid breaking the integrity verification or re-sign the assembly

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

## License

* [LGPL-3.0](https://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License)
