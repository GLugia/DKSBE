using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_1B : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public ushort unk_03;
		public ushort unk_04;
		public ushort unk_05;
		public ushort unk_06;
		public ushort unk_07;
		public ushort unk_08;
		public ushort unk_09;
		public ushort unk_0A;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadUInt16(origin, ref offset);
			this.unk_04 = ReadUInt16(origin, ref offset);
			this.unk_05 = ReadUInt16(origin, ref offset);
			this.unk_06 = ReadUInt16(origin, ref offset);
			this.unk_07 = ReadUInt16(origin, ref offset);
			this.unk_08 = ReadUInt16(origin, ref offset);
			this.unk_09 = ReadUInt16(origin, ref offset);
			this.unk_0A = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
