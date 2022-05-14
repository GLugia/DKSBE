using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Shield_Descriptions : ITableObject
	{
		public List<string>? descriptions;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.descriptions = new();
			int start = ReadInt32(origin, ref offset);
			int end = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			for (offset = start; offset < end;)
			{
				string str = ReadString(origin, ref offset);
				this.descriptions?.Add(str);
			}
			offset = ret_offset;
			return new() // new dictionary
			{
				{
					start,
					new() // new ptr_data
					{
						value = this.descriptions,
						size = end - start
					}
				}
			};
		}
	}
}
