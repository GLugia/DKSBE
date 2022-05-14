using DKSBE.data;
using DKSBE.data.system;
using DKSBE.data.types;

using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DKSBE
{
	public static unsafe class MarshalHelper
	{
		public static int PeekInt32(byte* origin, long offset)
		{
			return *(int*)(origin + offset);
		}

		private static byte[] ReadBytes(byte* origin, ref long offset, long amount)
		{
			byte[] bytes = new byte[amount];
			long locked_offset = offset;
			for (; offset < locked_offset + amount; offset++)
			{
				bytes[offset - locked_offset] = *(origin + offset);
			}
			return bytes;
		}

		public static T[] ReadValueTerminatedArray<T>(byte* origin, ref long offset, T end_value, bool align = true, int alignment = sizeof(int)) where T : struct
		{
			Type type = typeof(T);
			Array array = Array.CreateInstance(type, 0);
			do
			{
				Array narray = Array.CreateInstance(typeof(T), array.Length + 1);
				array.CopyTo(narray, 0);
				narray.SetValue(type.Name.ToLowerInvariant() switch
				{
					"byte" => ReadByte(origin, ref offset),
					"sbyte" => ReadSByte(origin, ref offset),
					"uint16" => ReadUInt16(origin, ref offset),
					"int16" => ReadInt16(origin, ref offset),
					"uint32" => ReadUInt32(origin, ref offset),
					"int32" => ReadInt32(origin, ref offset),
					"uint64" => ReadUInt64(origin, ref offset),
					"int64" => ReadInt64(origin, ref offset),
					"single" => ReadSingle(origin, ref offset),
					"double" => ReadDouble(origin, ref offset),
					"char" => ReadChars(origin, ref offset, 1)[0],
					"string" => ReadString(origin, ref offset),
					_ => throw new Exception($"Invalid type: {type}")
				}, array.Length);
				array = narray;
			}
			while (!(((T?)array.GetValue(array.Length - 1))?.Equals(end_value)).GetValueOrDefault());
			if (align && offset % alignment != 0)
			{
				offset += alignment - (offset % alignment);
			}
			return (T[])array;
		}

		public static T[] ReadArray<T>(byte* origin, ref long offset, long amount, bool align = true, int alignment = sizeof(int)) where T : struct
		{
			Array array = Array.CreateInstance(typeof(T), amount);
			for (int i = 0; i < amount; i++)
			{
				array.SetValue(typeof(T).Name.ToLowerInvariant() switch
				{
					"byte" => ReadByte(origin, ref offset),
					"sbyte" => ReadSByte(origin, ref offset),
					"uint16" => ReadUInt16(origin, ref offset),
					"int16" => ReadInt16(origin, ref offset),
					"uint32" => ReadUInt32(origin, ref offset),
					"int32" => ReadInt32(origin, ref offset),
					"uint64" => ReadUInt64(origin, ref offset),
					"int64" => ReadInt64(origin, ref offset),
					"single" => ReadSingle(origin, ref offset),
					"double" => ReadDouble(origin, ref offset),
					"char" => ReadChars(origin, ref offset, 1)[0],
					"string" => ReadString(origin, ref offset),
					_ => throw new Exception($"Invalid type: {typeof(T)}")
				}, i);
			}
			if (align && offset % alignment != 0)
			{
				offset += alignment - (offset % alignment);
			}
			return (T[])array;
		}

		public static byte ReadByte(byte* origin, ref long offset)
		{
			byte ret = *(origin + offset);
			offset++;
			return ret;
		}

		public static sbyte ReadSByte(byte* origin, ref long offset)
		{
			sbyte ret = *(sbyte*)(origin + offset);
			offset++;
			return ret;
		}

		public static ushort ReadUInt16(byte* origin, ref long offset)
		{
			ushort ret = *(ushort*)(origin + offset);
			offset += sizeof(ushort);
			return ret;
		}

		public static short ReadInt16(byte* origin, ref long offset)
		{
			short ret = *(short*)(origin + offset);
			offset += sizeof(short);
			return ret;
		}

		public static uint ReadUInt32(byte* origin, ref long offset)
		{
			uint ret = *(uint*)(origin + offset);
			offset += sizeof(uint);
			return ret;
		}

		public static int ReadInt32(byte* origin, ref long offset)
		{
			int ret = *(int*)(origin + offset);
			offset += sizeof(int);
			return ret;
		}

		public static ulong ReadUInt64(byte* origin, ref long offset)
		{
			ulong ret = *(ulong*)(origin + offset);
			offset += sizeof(ulong);
			return ret;
		}

		public static long ReadInt64(byte* origin, ref long offset)
		{
			long ret = *(long*)(origin + offset);
			offset += sizeof(long);
			return ret;
		}

		public static float ReadSingle(byte* origin, ref long offset)
		{
			float ret = *(float*)(origin + offset);
			offset += sizeof(float);
			return ret;
		}

		public static double ReadDouble(byte* origin, ref long offset)
		{
			double ret = *(double*)(origin + offset);
			offset += sizeof(double);
			return ret;
		}

		public static char[] ReadChars(byte* origin, ref long offset, long amount)
		{
			byte[] bytes = ReadBytes(origin, ref offset, amount);
			return Program.shift_jis?.GetChars(bytes) ?? Encoding.ASCII.GetChars(bytes);
		}
		
		public static string ReadString(byte* origin, ref long offset, int alignment = sizeof(int))
		{
			int length = 0;
			while (*(origin + offset + length) != 0)
			{
				length++;
			}
			string ret = Program.shift_jis?.GetString(origin + offset, length) ?? Encoding.ASCII.GetString(origin + offset, length);
			offset += length;
			offset += alignment - (offset % alignment);
			return ret;
		}

		public static Dictionary<long, PtrData?>? ReadSystemObject(byte* origin, ref long offset, out byte handler, out ITableObject? obj)
		{
			Dictionary<long, PtrData?>? result = new();
			// read the handler id
			handler = ReadByte(origin, ref offset);
			if (handler <= 0 || handler > system_types.Length - 1)
			{
				Logger.Error($"Invalid Handler ID: {handler.ToHex()} at {((int)offset - 1).ToHex()}");
				// stop reading
				throw new Exception();
			}
			// if the handler is 0 or can't be parsed to a type
			if (system_types[handler] == null)
			{
				Logger.Error($"Invalid Handler ID: {handler.ToHex()} at {((int)offset - 1).ToHex()}");
				// stop reading
				throw new Exception();
			}
			// create an instance of the type
			obj = (ITableObject)(Activator.CreateInstance(system_types[handler] ?? typeof(Dummy_File)) ?? new Dummy_File());
			FieldInfo? id = obj.GetType().GetField("id");
			if (id != null)
			{
				id.SetValue(obj, handler);
			}
			// dynamically call the Read function to let the class itself handle its own data
			Dictionary<long, PtrData?>? sections_to_skip = obj?.Read(origin, ref offset);
			// iterate all pointer references and add each unique instance to the dictionary
			if (sections_to_skip != null)
			{
				foreach ((long ptr, PtrData? data) in sections_to_skip)
				{
					if (!result.ContainsKey(ptr))
					{
						result.Add(ptr, data);
					}
				}
			}
			return result;
		}

		public static readonly Type?[] system_types = new Type?[0x70]
		{
			/* 0x00 */ typeof(Dummy_File),
			/* 0x01 */ typeof(Sys_01),
			/* 0x02 */ typeof(Sys_02),
			/* 0x03 */ typeof(Sys_03),
			/* 0x04 */ typeof(Sys_04),
			/* 0x05 */ typeof(Sys_05),
			/* 0x06 */ typeof(Sys_06),
			/* 0x07 */ typeof(Sys_07),
			/* 0x08 */ typeof(Sys_08),
			/* 0x09 */ typeof(Sys_09),
			/* 0x0A */ typeof(Sys_0A),
			/* 0x0B */ typeof(Sys_0B),
			/* 0x0C */ typeof(Sys_0C),
			/* 0x0D */ typeof(Sys_0D),
			/* 0x0E */ typeof(Sys_0E),
			/* 0x0F */ typeof(Sys_0F),
			/* 0x10 */ typeof(Sys_10),
			/* 0x11 */ typeof(Sys_11),
			/* 0x12 */ typeof(Sys_12),
			/* 0x13 */ typeof(Sys_13),
			/* 0x14 */ typeof(Sys_14),
			/* 0x15 */ null,
			/* 0x16 */ typeof(Sys_16),
			/* 0x17 */ typeof(Sys_17),
			/* 0x18 */ null,
			/* 0x19 */ typeof(Sys_19),
			/* 0x1A */ typeof(Sys_1A),
			/* 0x1B */ typeof(Sys_1B),
			/* 0x1C */ typeof(Sys_1C),
			/* 0x1D */ typeof(Sys_1D),
			/* 0x1E */ typeof(Sys_1E),
			/* 0x1F */ null,
			/* 0x20 */ typeof(Sys_20),
			/* 0x21 */ null,
			/* 0x22 */ typeof(Sys_22),
			/* 0x23 */ null,
			/* 0x24 */ null,
			/* 0x25 */ null,
			/* 0x26 */ null,
			/* 0x27 */ null,
			/* 0x28 */ null,
			/* 0x29 */ null,
			/* 0x2A */ null,
			/* 0x2B */ null,
			/* 0x2C */ null,
			/* 0x2D */ null,
			/* 0x2E */ null,
			/* 0x2F */ null,
			/* 0x30 */ null,
			/* 0x31 */ null,
			/* 0x32 */ null,
			/* 0x33 */ null,
			/* 0x34 */ null,
			/* 0x35 */ null,
			/* 0x36 */ null,
			/* 0x37 */ null,
			/* 0x38 */ null,
			/* 0x39 */ null,
			/* 0x3A */ null,
			/* 0x3B */ null,
			/* 0x3C */ null,
			/* 0x3D */ null,
			/* 0x3E */ null,
			/* 0x3F */ null,
			/* 0x40 */ null,
			/* 0x41 */ null,
			/* 0x42 */ null,
			/* 0x43 */ null,
			/* 0x44 */ null,
			/* 0x45 */ null,
			/* 0x46 */ null,
			/* 0x47 */ null,
			/* 0x48 */ null,
			/* 0x49 */ null,
			/* 0x4A */ null,
			/* 0x4B */ null,
			/* 0x4C */ null,
			/* 0x4D */ null,
			/* 0x4E */ null,
			/* 0x4F */ null,
			/* 0x50 */ null,
			/* 0x51 */ null,
			/* 0x52 */ null,
			/* 0x53 */ null,
			/* 0x54 */ null,
			/* 0x55 */ null,
			/* 0x56 */ null,
			/* 0x57 */ null,
			/* 0x58 */ null,
			/* 0x59 */ null,
			/* 0x5A */ null,
			/* 0x5B */ null,
			/* 0x5C */ null,
			/* 0x5D */ null,
			/* 0x5E */ null,
			/* 0x5F */ null,
			/* 0x60 */ null,
			/* 0x61 */ null,
			/* 0x62 */ null,
			/* 0x63 */ typeof(Sys_63),
			/* 0x64 */ null,
			/* 0x65 */ null,
			/* 0x66 */ null,
			/* 0x67 */ null,
			/* 0x68 */ null,
			/* 0x69 */ null,
			/* 0x6A */ null,
			/* 0x6B */ null,
			/* 0x6C */ null,
			/* 0x6D */ null,
			/* 0x6E */ null,
			/* 0x6F */ null,
		};
	}
}
