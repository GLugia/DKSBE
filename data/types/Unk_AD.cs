using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AD : ITableObject
	{
		public List<Unk_AD_Data?>? objects;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.objects = new();
			Dictionary<long, PtrData?>? result = new();
			int start = ReadInt32(origin, ref offset);
			int end = ReadInt32(origin, ref offset);
			uint[] ptrs = Array.Empty<uint>();
			uint temp_ptr;
			while ((temp_ptr = ReadUInt32(origin, ref offset)) != 0)
			{
				Array.Resize(ref ptrs, ptrs.Length + 1);
				ptrs[^1] = temp_ptr;
			}
			long ret_offset = offset;
			for (offset = start; offset < end;)
			{
				for (int i = 0; i < ptrs.Length - 1; i++)
				{
					if (!this.Read(origin, ref offset, ptrs[i], ptrs[i + 1], result))
					{
						continue;
					}
				}
			}
			this.Read(origin, ref offset, ptrs[^1], end, result);
			offset = ret_offset;
			return result;
		}

		private bool Read(byte* origin, ref long offset, long start, long end, Dictionary<long, PtrData?>? result)
		{
			if ((Stagebase.current_ptr_data?.ContainsKey(start)).GetValueOrDefault())
			{
				this.objects?.Add((Unk_AD_Data?)Stagebase.current_ptr_data?[start]?.value);
				offset += Stagebase.current_ptr_data?[start]?.size ?? 0;
				return false;
			}
			if ((result?.ContainsKey(start)).GetValueOrDefault())
			{
				this.objects?.Add((Unk_AD_Data?)result?[start]?.value);
				offset += result?[start]?.size ?? 0;
				return false;
			}
			offset = start;
			Unk_AD_Data data = new(end);
			Dictionary<long, PtrData?>? sub_result = data.Read(origin, ref offset);
			if (sub_result != null)
			{
				foreach ((long ptr, PtrData? ptr_data) in sub_result)
				{
					if (!(result?.ContainsKey(ptr)).GetValueOrDefault())
					{
						result?.Add(ptr, ptr_data);
					}
				}
			}
			result?.Add(start, new()
			{
				value = data,
				size = offset - start
			});
			return true;
		}
	}
}
