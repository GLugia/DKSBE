
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_DA : ITableObject
	{
		public ushort id;
		public ushort unk_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
