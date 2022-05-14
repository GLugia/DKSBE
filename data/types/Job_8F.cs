using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_8F : ITableObject
	{
		public uint id;
		public Job_8F_Data? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.data = (Job_8F_Data?)Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			long ret_offset = offset;
			offset = ptr;
			this.data = new();
			this.data.Read(origin, ref offset);
			long size = offset - ret_offset;
			offset = ret_offset;
			return new()
			{
				{
					ptr,
					new()
					{
						value = this.data,
						size = size
					}
				}
			};
		}
	}
}
