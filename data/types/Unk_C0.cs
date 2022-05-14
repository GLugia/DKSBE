using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_C0 : ITableObject
	{
		public List<string?>? text;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?>? result = new();
			this.text = new();
			int start = ReadInt32(origin, ref offset);
			int _ = ReadInt32(origin, ref offset); // unused value declaring the end of the data
			long ret_offset = offset;
			offset = start;
			int[] ptrs = Array.Empty<int>();
			do
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = ReadInt32(origin, ref offset);
			}
			while (PeekInt32(origin, offset) != 0);
			for (int i = 0; i < ptrs.Length; i++)
			{
				offset = ptrs[i];
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					this.text.Add((string?)Stagebase.current_ptr_data?[offset]?.value);
					offset += Stagebase.current_ptr_data?[offset]?.size ?? 0;
					continue;
				}
				if (result.ContainsKey(offset))
				{
					this.text.Add((string?)result[offset]?.value);
					offset += result[offset]?.size ?? 0;
					continue;
				}
				string value = ReadString(origin, ref offset);
				this.text.Add(value);
				result.Add(ptrs[i], new()
				{
					value = value,
					size = offset - ptrs[i]
				});
			}
			offset = ret_offset;
			return result;
		}
	}
}
