
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AC_Data : ITableObject
	{
		public uint unk_00;
		public uint unk_04;
		public uint unk_08;
		public uint unk_0C;
		public uint unk_10;
		public uint unk_14;
		public uint unk_18;
		public uint unk_1C;
		public Dictionary<uint, string>? dictionary;
		public Dictionary<ushort, Unk_AC_Sub_Data>? sub_dictionary;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadUInt32(origin, ref offset);
			this.unk_04 = ReadUInt32(origin, ref offset);
			this.unk_08 = ReadUInt32(origin, ref offset);
			this.unk_0C = ReadUInt32(origin, ref offset);
			this.unk_10 = ReadUInt32(origin, ref offset);
			this.unk_14 = ReadUInt32(origin, ref offset);
			this.unk_18 = ReadUInt32(origin, ref offset);
			this.unk_1C = ReadUInt32(origin, ref offset);
			this.dictionary = new();
			this.sub_dictionary = new();
			while (ReadUInt32(origin, ref offset) == 1)
			{
				uint key = ReadUInt32(origin, ref offset);
				string value = ReadString(origin, ref offset);
				this.dictionary.Add(key, value);
			}
			while (ReadUInt32(origin, ref offset) == 1)
			{
				ushort key = ReadUInt16(origin, ref offset);
				Unk_AC_Sub_Data value = new();
				value.Read(origin, ref offset);
				this.sub_dictionary.Add(key, value);
			}
			return null;
		}
	}
}
