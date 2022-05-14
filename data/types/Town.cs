
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Town : ITableObject
	{
		public ushort id;
		public short unk_00;
		public string? name;
		public int unk_01;
		public int value;
		public int unk_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_01 = ReadInt32(origin, ref offset);
			this.value = ReadInt32(origin, ref offset);
			this.unk_02 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
