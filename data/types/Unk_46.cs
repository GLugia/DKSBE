using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_46 : ITableObject
	{
		public ushort id;
		public ushort unk_00;
		public byte unk_01;
		public byte unk_02;
		public byte unk_03;
		public byte padding;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.padding = ReadByte(origin, ref offset);
			return null;
		}
	}
}
