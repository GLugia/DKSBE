using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Magic_Item : ITableObject
	{
		public ushort id;
		public short icon;
		public string? name;
		public int price;
		public short power;
		public byte magic_type;
		public byte unk_00;
		public ushort unk_enum_01;
		public ushort unk_enum_02;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.icon = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.price = ReadInt32(origin, ref offset);
			this.power = ReadInt16(origin, ref offset);
			this.magic_type = ReadByte(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.unk_enum_01 = ReadUInt16(origin, ref offset);
			this.unk_enum_02 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
