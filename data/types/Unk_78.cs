
using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_78 : ITableObject
	{
		public uint id;
		public List<Unk_78_Data>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.data = new();
			int ptr = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = ptr;
			Dictionary<long, PtrData?>? ret = new();
			int[] ptrs = Array.Empty<int>();
			do
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = ReadInt32(origin, ref offset);
			}
			while (ptrs[^1] != 0);
			for (int i = 0; i < ptrs.Length; i++)
			{
				offset = ptrs[i];
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					this.data.Add((Unk_78_Data)(Stagebase.current_ptr_data?[offset]?.value ?? new Unk_78_Data()));
					continue;
				}
				Unk_78_Data sub = new();
				sub.Read(origin, ref offset);
				this.data.Add(sub);
				ret.Add(ptrs[i], new()
				{
					value = sub,
					size = offset - ptrs[i]
				});
			}
			offset = ret_offset;
			return ret;
		}
	}
}
