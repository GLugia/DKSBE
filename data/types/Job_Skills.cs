using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Skills : ITableObject
	{
		public byte id;
		public byte gender;
		public byte padding_0;
		public byte level_2;
		public byte level_4;
		public byte field_skill_chance;
		public byte field_skill_max;
		public byte padding_1;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.padding_0 = ReadByte(origin, ref offset);
			this.level_2 = ReadByte(origin, ref offset);
			this.level_4 = ReadByte(origin, ref offset);
			this.field_skill_chance = ReadByte(origin, ref offset);
			this.field_skill_max = ReadByte(origin, ref offset);
			this.padding_1 = ReadByte(origin, ref offset);
			return null;
		}
	}
}
