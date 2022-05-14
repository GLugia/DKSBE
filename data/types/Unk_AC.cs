
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AC : ITableObject
	{
		public uint id;
		public Unk_AC_Data? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = ptr;
			this.data = new();
			this.data.Read(origin, ref offset);
			offset = ret_offset;
			return null;
		}
	}
}
