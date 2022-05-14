
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_85 : ITableObject
	{
		public List<byte[]?>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int[] ptrs = Array.Empty<int>();
			do
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = ReadInt32(origin, ref offset);
			}
			while (offset < ptrs[0]);
			this.data = new();
			for (int i = 0; i < ptrs.Length - 1; i++)
			{
				offset = ptrs[i];
				byte[] bytes = Array.Empty<byte>();
				do
				{
					Array.Resize(ref bytes, bytes.Length + 1);
					bytes[^1] = ReadByte(origin, ref offset);
				}
				while (bytes[^1] != 0);
				this.data.Add(bytes);
			}
			offset = ptrs[^1];
			return null;
		}
	}
}
