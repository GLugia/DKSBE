using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Model_0_3 : ITableObject
	{
		public byte id;
		public byte gender;
		public short padding;
		public string? f0;
		public string? k0;
		public string? fg0;
		public string? fg1;
		public string? fg2;
		public string? fg3;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			this.f0 = ReadString(origin, ref offset);
			this.k0 = ReadString(origin, ref offset);
			this.fg0 = ReadString(origin, ref offset);
			this.fg1 = ReadString(origin, ref offset);
			this.fg2 = ReadString(origin, ref offset);
			this.fg3 = ReadString(origin, ref offset);
			return null;
		}
	}
}
