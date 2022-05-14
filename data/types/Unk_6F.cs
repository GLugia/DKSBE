
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_6F : ITableObject
	{
		public byte id;
		public byte unk_01;
		public short padding;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
