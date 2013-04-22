Bit Conversion
===

**libWyvernzora** features an extensive tool kit for converting numeric value to and from their binary forms. The main class that does the trick is `libWyvernzora.Core.DirectIntConv`.

Signed-Unsigned Direct Conversion
---
``` C#
using System;
using libWyvernzora.Core;

public static class Program
{
	public static void Main(String[] args)
	{
		{
			// Unsigned -> Signed
			Byte unsigned = 255;	// 0xFF
			SByte signed = unsigned.Sign();

			Console.WriteLine(signed);
			// Output: -1
		}

		{
			// Signed -> Unsigned
			Int16 signed = -1;	// 0xFFFF
			UInt16 unsigned = signed.Unsign();

			Console.WriteLine(unsigned);
			// Output: 65535
		}
	}
}
```

Binary-Numeric Direct Conversion
---
``` C#
using System;
using libWyvernzora.Core;

public static class Program
{
	public static void Main(String[] args)
	{
		{
			// Numeric -> Binary
			Int32 number = 12345;
			Byte[] bigEndian = number.ToBinary(BitSequence.BigEndian);
			Byte[] littleEndian = number.ToBinary(BitSequence.LittleEndian);

			// bigEndian:		00 00 48 57
			// littleEndian:	57 48 00 00
		}
		{
			// Binary -> Numeric
			Byte[] binary = new[] { 0, 0, 48, 57 };
			Int32 bigEndian = binary.ToInt32(BitSequence.BigEndian);
			Int32 littleEndian = binary.ToInt32(BitSequence.LittleEndian);

			// bigEndian:		12345
			// littleEndian:	959447040
		}
	}
}
```

Number -> Hexadecimal String
---
``` C#
using System;
using libWyvernzora.Core;

public static class Program
{
	public static void Main(String[] args)
	{
		Int32 number = 12345;
		String hexString = number.ToHexString(8);
		
		Console.WriteLine(hexString);
		// Output: 00003039
	}
}
```