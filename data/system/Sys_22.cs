using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_22 : ITableObject
	{
		public byte id;
		public byte pad_01;
		public byte unk_02;
		public byte unk_03;
		public int unk_04;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.pad_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_04 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
