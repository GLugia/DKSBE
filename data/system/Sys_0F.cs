using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_0F : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort unk_01;
		public float unk_02;
		public float unk_03;
		public float unk_04;
		public float unk_05;
		public float unk_06;
		public float unk_07;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			this.unk_02 = ReadSingle(origin, ref offset);
			this.unk_03 = ReadSingle(origin, ref offset);
			this.unk_04 = ReadSingle(origin, ref offset);
			this.unk_05 = ReadSingle(origin, ref offset);
			this.unk_06 = ReadSingle(origin, ref offset);
			this.unk_07 = ReadSingle(origin, ref offset);
			return null;
		}
	}
}
