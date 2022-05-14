using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_4C : ITableObject
	{
		public uint id;
		public byte unk_00;
		public byte unk_01;
		public byte unk_02;
		public byte padding;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.padding = ReadByte(origin, ref offset);
			return null;
		}
	}
}
