
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Space_Effect : ITableObject
	{
		public byte id;
		public byte sub_id;
		public byte unk_02;
		public byte unk_03;
		public int unk_70;
		public int value;
		public string? display_value;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.sub_id = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_70 = ReadInt32(origin, ref offset);
			this.value = ReadInt32(origin, ref offset);
			this.display_value = ReadString(origin, ref offset);
			return null;
		}
	}
}
