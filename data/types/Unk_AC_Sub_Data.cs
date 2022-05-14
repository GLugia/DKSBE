
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AC_Sub_Data : ITableObject
	{
		public ushort unk_00;
		public uint unk_02;
		public uint unk_06;
		public uint unk_0A;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadUInt32(origin, ref offset);
			this.unk_06 = ReadUInt32(origin, ref offset);
			this.unk_0A = ReadUInt32(origin, ref offset);
			return null;
		}
	}
}
