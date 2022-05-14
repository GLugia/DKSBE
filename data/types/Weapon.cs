using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Weapon : ITableObject
	{
		public ushort id;
		public ushort icon;
		public string? name;
		public byte unk_00;
		public byte class_id;
		public byte unk_01;
		public byte rating;
		public int price;
		public short att;
		public short def;
		public short mag;
		public short spd;
		public short hp;
		public byte unk_02;
		public byte unk_03;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.icon = ReadUInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.class_id = ReadByte(origin, ref offset);
			this.unk_01 = ReadByte(origin, ref offset);
			this.rating = ReadByte(origin, ref offset);
			this.price = ReadInt32(origin, ref offset);
			this.att = ReadInt16(origin, ref offset);
			this.def = ReadInt16(origin, ref offset);
			this.mag = ReadInt16(origin, ref offset);
			this.spd = ReadInt16(origin, ref offset);
			this.hp = ReadInt16(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			return null;
		}
	}
}
