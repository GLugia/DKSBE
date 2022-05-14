
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_68 : ITableObject
	{
		public byte id;
		public byte unk_01;
		public short padding;
		public int value;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			this.value = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
