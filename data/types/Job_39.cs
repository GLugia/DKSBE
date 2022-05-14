using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_39 : ITableObject
	{
		public uint id;
		public Job_39_Data? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			// TODO: Convert to a reader method that uses an ref offset given as parameter and a generic of IObjectType
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.data = (Job_39_Data?)Stagebase.current_ptr_data?[ptr]?.value;
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
