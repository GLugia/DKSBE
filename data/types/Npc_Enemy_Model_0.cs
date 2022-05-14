using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Enemy_Model_0 : ITableObject
	{
		public uint id;
		public string? f0;
		public string? fg0;
		public Dictionary<int, Npc_Enemy_Model_0_Data?>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			Dictionary<long, PtrData?>? result = new();
			this.id = ReadUInt32(origin, ref offset);
			this.f0 = ReadString(origin, ref offset);
			this.fg0 = ReadString(origin, ref offset);
			this.data = new();
			while (ReadInt32(origin, ref offset) == 1)
			{
				int key = ReadInt32(origin, ref offset);
				int ptr = ReadInt32(origin, ref offset);
				if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
				{
					this.data.Add(key, (Npc_Enemy_Model_0_Data?)(Stagebase.current_ptr_data?[ptr]?.value ?? new Npc_Enemy_Model_0_Data()));
					continue;
				}
				if (result.ContainsKey(ptr))
				{
					this.data.Add(key, (Npc_Enemy_Model_0_Data?)result[ptr]?.value);
					continue;
				}
				long ret_offset = offset;
				offset = ptr;
				Npc_Enemy_Model_0_Data value = new();
				value.Read(origin, ref offset);
				result.Add(ptr, new()
				{
					value = value,
					size = offset - ptr
				});
				offset = ret_offset;
				this.data.Add(key, value);
			}
			return result;
		}
	}
}
