using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_6E : ITableObject
	{
		public uint id;
		public float x;
		public float y;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.x = ReadSingle(origin, ref offset);
			this.y = ReadSingle(origin, ref offset);
			return null;
		}
	}
}
