using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_1D : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort unk_01;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
