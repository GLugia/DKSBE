using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Stage : ITableObject
	{
		public uint id;
		public string? file;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.file = (string?)Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			long ret_offset = offset;
			offset = ptr;
			this.file = ReadString(origin, ref offset);
			long end = offset;
			offset = ret_offset;
			return new()
			{
				{
					ptr,
					new()
					{
						value = this.file,
						size = end - ptr
					}
				}
			};
		}
	}
}
