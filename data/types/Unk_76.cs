using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_76 : ITableObject
	{
		public Unk_76_Data? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.data = (Unk_76_Data?)Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			long ret_offset = offset;
			offset = ptr;
			this.data = new();
			this.data.Read(origin, ref offset);
			long length = offset - ptr;
			offset = ret_offset;
			return new()
			{
				{
					ptr,
					new()
					{
						value = this.data,
						size = length
					}
				}
			};
		}
	}
}
