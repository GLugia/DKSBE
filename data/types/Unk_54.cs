using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_54 : ITableObject
	{
		public ushort id;
		public ushort unk_00; // always 0x0100

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt16(origin, ref offset);
			this.unk_00 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
