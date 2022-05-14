
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_94 : ITableObject
	{
		public byte[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int start = ReadInt32(origin, ref offset);
			int pad = ReadInt32(origin, ref offset);
			this.data = Array.Empty<byte>();
			do
			{
				Array.Resize(ref this.data, this.data.Length + 1);
				this.data[^1] = ReadByte(origin, ref offset);
			}
			while (this.data[^1] != 0);
			offset = pad;
			return null;
		}
	}
}
