using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_05 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort padding;
		public uint length;
		public List<float[]?>? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.padding = ReadUInt16(origin, ref offset);
			this.length = ReadUInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.data = (List<float[]?>?)Stagebase.current_ptr_data?[ptr]?.value;
				return null;
			}
			Dictionary<long, PtrData?>? result = new();
			long ret_offset = offset;
			offset = ptr;
			this.data = new();
			for (int i = 0; i < this.length; i++)
			{
				if ((Stagebase.current_ptr_data?.ContainsKey(offset)).GetValueOrDefault())
				{
					this.data.Add((float[]?)Stagebase.current_ptr_data?[offset]?.value);
					offset += Stagebase.current_ptr_data?[offset]?.size ?? 0;
					continue;
				}
				if (result.ContainsKey(offset))
				{
					this.data.Add((float[]?)result[offset]?.value);
					offset += result[offset]?.size ?? 0;
					continue;
				}
				long locked_offset = offset;
				float[]? arr = ReadArray<float>(origin, ref offset, 0x60);
				this.data.Add(arr);
				result.Add(locked_offset, new()
				{
					value = arr,
					size = offset - locked_offset
				});
			}
			offset = ret_offset;
			return result;
		}
	}
}
