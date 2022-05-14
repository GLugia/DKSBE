
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_2B : ITableObject
	{
		public ushort id;
		public ushort unk_02;
		public ushort unk_04;
		public ushort unk_06;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadUInt16(origin, ref offset);
			this.unk_04 = ReadUInt16(origin, ref offset);
			this.unk_06 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
