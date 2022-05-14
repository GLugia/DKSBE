
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_7F : ITableObject
	{
		public uint id;
		public ushort[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{

			this.id = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.data = (ushort[]?)Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			long ret_offset = offset;
			offset = ptr;
			ushort count = ReadUInt16(origin, ref offset);
			this.data = new ushort[count];
			for (int i = 0; i < count; i++)
			{
				this.data[i] = ReadUInt16(origin, ref offset);
			}
			offset = ret_offset;
			return new()
			{
				{
					ptr,
					new()
					{
						value = this.data,
						size = this.data.Length * sizeof(ushort)
					}
				}
			};
		}
	}
}
