
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_DB : ITableObject
	{
		public int unk_00;
		public ushort[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadInt32(origin, ref offset);
			int end = ReadInt32(origin, ref offset);
			int pad = ReadInt32(origin, ref offset);
			this.data = Array.Empty<ushort>();
			while (offset < end)
			{
				Array.Resize(ref this.data, this.data.Length + 1);
				this.data[^1] = ReadUInt16(origin, ref offset);
			}
			offset = pad;
			return null;
		}
	}
}
