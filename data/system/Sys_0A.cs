using DKSBE.data.types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_0A : ITableObject
	{
		public byte unk_00;
		public ushort unk_01;
		public object? obj;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.obj = Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			Dictionary<long, PtrData?>? result = new();
			long ret_offset = offset;
			offset = ptr;
			// read the handler id
			Dictionary<long, PtrData?>? sub_result = ReadSystemObject(origin, ref offset, out byte _, out ITableObject? obj);
			// iterate all pointer references and add each unique instance to the dictionary
			if (sub_result != null)
			{
				foreach ((long sub_ptr, PtrData? data) in sub_result)
				{
					if (!result.ContainsKey(sub_ptr))
					{
						result.Add(sub_ptr, data);
					}
				}
			}
			this.obj = obj;
			offset = ret_offset;
			return result;
		}
	}
}
