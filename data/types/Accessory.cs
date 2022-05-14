using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Accessory : ITableObject
	{
		public ushort id;
		public ushort icon;
		public string? name;
		public int unk_00;
		public int price;
		public short att;
		public short def;
		public short mag;
		public short spd;
		public short hp;
		public byte unk_01;
		public byte unk_02;
		public int unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.icon = ReadUInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_00 = ReadInt32(origin, ref offset);
			this.price = ReadInt32(origin, ref offset);
			this.att = ReadInt16(origin, ref offset);
			this.def = ReadInt16(origin, ref offset);
			this.mag = ReadInt16(origin, ref offset);
			this.spd = ReadInt16(origin, ref offset);
			this.hp = ReadInt16(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadInt32(origin, ref offset);
			return null;
		}
	}
}
