using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Item_Gift : ITableObject
	{
		public ushort id;
		public short unk_00;
		public string? name;
		public int price;
		public int gift_value;
		public int unk_01;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.price = ReadInt32(origin, ref offset);
			this.gift_value = ReadInt32(origin, ref offset);
			this.unk_01 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
