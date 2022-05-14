using DKSBE.data.system;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_AF : ITableObject
	{
		public SortedDictionary<byte, List<ITableObject?>?>? _objects;

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
			int start = ReadInt32(origin, ref offset);
			int end = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			for (offset = start; offset < end;)
			{
				if (result.ContainsKey(offset))
				{
					offset += result[offset]?.size ?? 0;
					continue;
				}
				Dictionary<long, PtrData?>? sub_result = ReadSystemObject(origin, ref offset, out byte handler, out ITableObject? obj);
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
				// create a ref of the resulting object and store it in the dictionary
				this._objects?[handler]?.Add(obj);
			}
			offset = ret_offset;
			return result;
		}
	}
}
