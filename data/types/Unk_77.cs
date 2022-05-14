using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_77 : ITableObject
	{
		public List<ushort[]?>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?> result = new();
			int ptr = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			offset = ptr;
			offset -= sizeof(int) * 2;
			int end = ReadInt32(origin, ref offset);
			offset += sizeof(int);
			int[] ptrs = Array.Empty<int>();
			do
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = ReadInt32(origin, ref offset);
			}
			while (PeekInt32(origin, offset) != 0);
			Array.Resize(ref ptrs, ptrs.Length + 1);
			ptrs[^1] = end;
			this.data = new();
			for (int i = 0; i < ptrs.Length - 1; i++)
			{
				offset = ptrs[i];
				ushort[] array = ReadArray<ushort>(origin, ref offset, (ptrs[i + 1] - ptrs[i]) / sizeof(ushort), false);
				this.data.Add(array);
				result.Add(ptrs[i], new()
				{
					value = array,
					size = offset - ptrs[i]
				});
			}
			offset = ret_offset;
			return result;
		}
	}
}
