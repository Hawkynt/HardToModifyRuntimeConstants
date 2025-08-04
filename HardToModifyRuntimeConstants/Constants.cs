using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HardToModifyRuntimeConstants;

public static class Constants {
  
  private readonly struct ConstantContainer() {
    public readonly ulong Pi = ~0x400921FB54442D18UL;
    public readonly ulong E = ~0x4005BF0A8B145769UL;
    public readonly ulong Sqrt2 = ~0x3FF6A09E667F3BCCUL;
  }

  private static readonly long _storage;
  private static readonly long _storageKey = Random.Shared.NextInt64();
  private const long _pepper = unchecked((long)0xdeadbeefcaffee42);

  static Constants() {
    ConstantContainer container = new();
    var pointer=GCHandle.Alloc(container,GCHandleType.Pinned).AddrOfPinnedObject().ToInt64();
    _storage = pointer ^ _storageKey ^ _pepper;
  }

  public static unsafe double Pi => Unsafe.BitCast<ulong,double>(~((ConstantContainer*)(_storage ^ _storageKey ^ _pepper))->Pi);
  public static unsafe double E => Unsafe.BitCast<ulong, double>(~((ConstantContainer*)(_storage ^ _storageKey ^ _pepper))->E);
  public static unsafe double Sqrt2 => Unsafe.BitCast<ulong, double>(~((ConstantContainer*)(_storage ^ _storageKey ^ _pepper))->Sqrt2);


}