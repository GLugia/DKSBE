using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.system
{
	public unsafe class Sys_03 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public byte unk_04;
		public byte unk_05;
		public byte pad_06;
		public float unk_07;
		public float unk_08;
		[Unused]
		public ushort unk_09;
		public ushort unk_0A;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.unk_04 = ReadByte(origin, ref offset);
			this.unk_05 = ReadByte(origin, ref offset);
			this.pad_06 = ReadByte(origin, ref offset);
			this.unk_07 = ReadSingle(origin, ref offset);
			this.unk_08 = ReadSingle(origin, ref offset);
			this.unk_09 = ReadUInt16(origin, ref offset);
			this.unk_0A = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
