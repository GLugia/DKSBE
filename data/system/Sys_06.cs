using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_06 : ITableObject
	{
		public byte id;
		public ushort unk_00;
		public byte pad_01;
		public int? sometimes_value;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.pad_01 = ReadByte(origin, ref offset);
			int val = ReadInt32(origin, ref offset);
			if (val == 0x10)
			{
				this.sometimes_value = val;
			}
			else
			{
				offset -= sizeof(int);
			}
			return null;
		}
	}
}
