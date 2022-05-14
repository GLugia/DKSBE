using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Item : ITableObject
	{
		public byte id;
		public byte type;
		public short icon;
		public string? name;
		public int price;
		public byte unk_00;
		public byte unk_01;
		public short padding;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.type = ReadByte(origin, ref offset);
			this.icon = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.price = ReadInt32(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
