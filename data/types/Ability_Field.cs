using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Ability_Field : ITableObject
	{
		public byte id;
		public byte accuracy;
		[Unused]
		public byte unk_02; // unused
		[Unused]
		public byte unk_03; // unused
		public string? name;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.accuracy = ReadByte(origin, ref offset);
			this.unk_02 = ReadByte(origin, ref offset);
			this.unk_03 = ReadByte(origin, ref offset);
			this.name = ReadString(origin, ref offset);
			return null;
		}
	}
}
