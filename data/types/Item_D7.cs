using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Item_D7 : ITableObject
	{
		public byte id;
		public byte unk_00;
		public short unk_01;
		public int unk_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadInt16(origin, ref offset);
			this.unk_02 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
