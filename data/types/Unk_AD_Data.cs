using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AD_Data : ITableObject
	{
		public SortedDictionary<byte, List<ITableObject?>?>? _objects;

		private long end;

		public Unk_AD_Data(long end)
		{
			this.end = end;
		}

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this._objects = new();
			for (byte b = 0; b < system_types.Length; b++)
			{
				if (system_types[b] != null)
				{
					this._objects.Add(b, new());
				}
			}
			Dictionary<long, PtrData?>? result = new();
			// TODO: find where this constant is declared
			int[] ptrs = new int[0x13];
			for (int i = 0; i < ptrs.Length; i++)
			{
				ptrs[i] = ReadInt32(origin, ref offset);
			}
			for (int i = 0; i < ptrs.Length - 1;
				i++)
			{
				this.Handle(origin, ref offset, ptrs[i], ptrs[i + 1], result);
			}
			this.Handle(origin, ref offset, ptrs[^1], this.end, result);
			this.end = 0;
			return result;
		}

		private void Handle(byte* origin, ref long offset, long start, long end, Dictionary<long, PtrData?> result)
		{
			for (offset = start; offset < end;)
			{
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					offset += Stagebase.current_ptr_data?[offset]?.size ?? 0;
					continue;
				}
				if (result.ContainsKey(offset))
				{
					offset += result[offset]?.size ?? 0;
					continue;
				}
				Dictionary<long, PtrData?>? sub_result = ReadSystemObject(origin, ref offset, out byte handler, out ITableObject? obj);
				// iterate all pointer references and add each unique instance to the dictionary
				if (sub_result != null)
				{
					foreach ((long ptr, PtrData? data) in sub_result)
					{
						if (!result.ContainsKey(ptr))
						{
							result.Add(ptr, data);
						}
					}
				}
				// create a ref of the resulting object and store it in the dictionary
				this._objects?[handler]?.Add(obj);
			}
			return;
		}
	}
}
