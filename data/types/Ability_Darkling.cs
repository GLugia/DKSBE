using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Ability_Darkling : ITableObject
	{
		public byte id;
		public byte day_duration;
		public short cost;
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.day_duration = ReadByte(origin, ref offset);
			this.cost = ReadInt16(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
