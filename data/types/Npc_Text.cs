using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Text : ITableObject
	{
		public uint id;
		public List<string?>? list;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?>? result = new();
			this.id = ReadUInt32(origin, ref offset);
			this.list = new();
			int[] ptrs = Array.Empty<int>();
			int temp;
			while ((temp = ReadInt32(origin, ref offset)) != 0)
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = temp;
			}
			long ret_offset = offset;
			for (int i = 0; i < ptrs.Length; i++)
			{
				offset = ptrs[i];
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					this.list?.Add((string?)Stagebase.current_ptr_data?[offset]?.value);
					continue;
				}
				if (result.ContainsKey(offset))
				{
					this.list?.Add((string?)result[offset]?.value);
					continue;
				}
				string str = ReadString(origin, ref offset);
				this.list?.Add(str);
				result.Add(offset, new()
				{
					value = str,
					size = offset - ptrs[i]
				});
			}
			offset = ret_offset;
			return result;
		}
	}
}
