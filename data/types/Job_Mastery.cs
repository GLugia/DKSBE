using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Mastery : ITableObject
	{
		public byte id;
		public byte gender;
		public short level_att;
		public short level_def;
		public short level_mag;
		public short level_spd;
		public short level_hp;
		public short mastery_att;
		public short mastery_def;
		public short mastery_mag;
		public short mastery_spd;
		public short mastery_hp;
		public short padding;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.level_att = ReadInt16(origin, ref offset);
			this.level_def = ReadInt16(origin, ref offset);
			this.level_mag = ReadInt16(origin, ref offset);
			this.level_spd = ReadInt16(origin, ref offset);
			this.level_hp = ReadInt16(origin, ref offset);
			this.mastery_att = ReadInt16(origin, ref offset);
			this.mastery_def = ReadInt16(origin, ref offset);
			this.mastery_mag = ReadInt16(origin, ref offset);
			this.mastery_spd = ReadInt16(origin, ref offset);
			this.mastery_hp = ReadInt16(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
