using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_02 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort[]? data;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			long locked_offset = offset - 1;
			this.unk_00 = ReadByte(origin, ref offset);
			this.data = ReadArray<ushort>(origin, ref offset, 0x01, false);
			return new()
			{
				{
					locked_offset,
					new()
					{
						value = this,
						size = offset - locked_offset
					}
				}
			};
		}
	}
}
