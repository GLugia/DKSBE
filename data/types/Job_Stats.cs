using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Stats : ITableObject
	{
		public byte id;
		public byte gender;
		public short att;
		public short def;
		public short mag;
		public short spd;
		public short hp;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.att = ReadInt16(origin, ref offset);
			this.def = ReadInt16(origin, ref offset);
			this.mag = ReadInt16(origin, ref offset);
			this.spd = ReadInt16(origin, ref offset);
			this.hp = ReadInt16(origin, ref offset);
			return null;
		}
	}
}
