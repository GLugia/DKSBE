using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_4D : ITableObject
	{
		public List<Unk_4D_Data?>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?> result = new();
			int ptr = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = ptr;
			int[] ptrs = Array.Empty<int>();
			do
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = ReadInt32(origin, ref offset);
			}
			while (PeekInt32(origin, offset) != 0);
			this.data = new();
			for (int i = 0; i < ptrs.Length; i++)
			{
				offset = ptrs[i];
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					this.data.Add((Unk_4D_Data?)Stagebase.current_ptr_data?[offset]?.value);
					continue;
				}
				if (result.ContainsKey(offset))
				{
					this.data.Add((Unk_4D_Data?)result[offset]?.value);
					continue;
				}
				Unk_4D_Data value = new();
				value.Read(origin, ref offset);
				this.data.Add(value);
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
