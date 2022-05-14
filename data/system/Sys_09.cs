using DKSBE.data.types;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_09 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public bool contains_two_pointers;
		public ReadType read_type;

		public byte pad_03;
		public short pad_04;
		public object? ptr_data_0;
		public object? ptr_data_1;

		public enum ReadType : byte
		{
			NULL,
			BLOCK_READER = 0x01,
			U16_ZERO_TERMINATED_ARRAY_READER = 0x02,
			IMPOSSIBLE_00 = 0xFD,
			IMPOSSIBLE_01 = 0xFE,
			COUNT
		}

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.contains_two_pointers = ReadByte(origin, ref offset) == 0;
			this.read_type = (ReadType)ReadInt32(origin, ref offset);
			int ptr = ReadInt32(origin, ref offset);
			Dictionary<long, PtrData?>? result = new();
			long ret_offset = offset;
			if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
			{
				this.ptr_data_0 = Stagebase.current_ptr_data?[ptr]?.value;
			}
			else
			{
				offset = ptr;
				this.ptr_data_0 = this.Read(origin, ref offset, result);
			}
			if (this.contains_two_pointers)
			{
				offset = ret_offset;
				ptr = ReadInt32(origin, ref offset);
				if ((Stagebase.current_ptr_data?.ContainsKey(ptr)).GetValueOrDefault())
				{
					this.ptr_data_1 = Stagebase.current_ptr_data?[ptr]?.value;
				}
				else
				{
					ret_offset = offset;
					offset = ptr;
					this.ptr_data_1 = this.Read(origin, ref offset, result);
				}
			}
			offset = ret_offset;
			return result;
		}

		private object? Read(byte* origin, ref long offset, Dictionary<long, PtrData?>? result)
		{
			object? ret = null;
			switch (this.read_type)
			{
				case (ReadType)0x38:
				case (ReadType)0x40:
				case ReadType.U16_ZERO_TERMINATED_ARRAY_READER:
					{
						ret = ReadValueTerminatedArray<ushort>(origin, ref offset, 0);
						break;
					}
				default:
					{
						if ((result?.ContainsKey(offset)).GetValueOrDefault())
						{
							this.ptr_data_0 = result?[offset]?.value;
							offset += result?[offset]?.size ?? 0;
							break;
						}
						Dictionary<long, PtrData?>? sub_result = ReadSystemObject(origin, ref offset, out byte _, out ITableObject? obj);
						ret = obj;
						if (sub_result != null)
						{
							foreach ((long sub_ptr, PtrData? ptr_data) in sub_result)
							{
								if (!(result?.ContainsKey(sub_ptr)).GetValueOrDefault())
								{
									result?.Add(sub_ptr, ptr_data);
								}
							}
						}
						break;
					}
			}
			return ret;
		}
	}
}
