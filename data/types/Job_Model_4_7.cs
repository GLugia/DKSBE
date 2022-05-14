using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Job_Model_4_7 : ITableObject
	{
		public byte id;
		public byte gender;
		public short padding;
		public string? fg4;
		public string? fg5;
		public string? fg6;
		public string? fg7;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadByte(origin, ref offset);
			this.gender = ReadByte(origin, ref offset);
			this.padding = ReadInt16(origin, ref offset);
			this.fg4 = ReadString(origin, ref offset);
			this.fg5 = ReadString(origin, ref offset);
			this.fg6 = ReadString(origin, ref offset);
			this.fg7 = ReadString(origin, ref offset);
			return null;
		}
	}
}
