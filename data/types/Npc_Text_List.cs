using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Text_List : ITableObject
	{
		public List<string?>? list;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?>? result = new();
			this.list = new();
			int start = ReadInt32(origin, ref offset);
			int end = ReadInt32(origin, ref offset);
			long ret_offset = offset;
			for (offset = start; offset < end;)
			{
				long locked = offset;
				if ((Stagebase.current_ptr_data?.ContainsKey(locked)).GetValueOrDefault())
				{
					this.list?.Add((string?)Stagebase.current_ptr_data?[locked]?.value);
					continue;
				}
				if (result.ContainsKey(locked))
				{
					this.list?.Add((string?)result[locked]?.value);
					continue;
				}
				string str = ReadString(origin, ref offset);
				this.list?.Add(str);
				result.Add(locked, new()
				{
					value = str,
					size = offset - locked
				});
			}
			offset = ret_offset;
			return result;
		}
	}
}
