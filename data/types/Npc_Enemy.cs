using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Npc_Enemy : ITableObject
	{
		public byte id;
		public byte unk_00;
		public ushort level;
		public string? name;
		public short hp;
		public short att;
		public short def;
		public short spd;
		public short mag;
		public short padding_00;
		public byte offensive_magic;
		public byte defensive_magic;
		public byte battle_skill;
		public byte padding_01;
		public short exp;
		public short gold;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.unk_00 = ReadByte(origin, ref offset);
			this.level = ReadUInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			this.hp = ReadInt16(origin, ref offset);
			this.att = ReadInt16(origin, ref offset);
			this.def = ReadInt16(origin, ref offset);
			this.spd = ReadInt16(origin, ref offset);
			this.mag = ReadInt16(origin, ref offset);
			this.padding_00 = ReadInt16(origin, ref offset);
			this.offensive_magic = ReadByte(origin, ref offset);
			this.defensive_magic = ReadByte(origin, ref offset);
			this.battle_skill = ReadByte(origin, ref offset);
			this.padding_01 = ReadByte(origin, ref offset);
			this.exp = ReadInt16(origin, ref offset);
			this.gold = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
