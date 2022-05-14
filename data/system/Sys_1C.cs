using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_1C : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort unk_01;
		public ushort unk_02;
		public ushort unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			long locked_offset = offset - 1;
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadUInt16(origin, ref offset);
			this.unk_03 = ReadUInt16(origin, ref offset);
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
