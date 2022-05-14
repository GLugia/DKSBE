using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_A4 : ITableObject
	{
		public ushort unk_00;
		public ushort unk_01;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadUInt16(origin, ref offset);
			this.unk_01 = ReadUInt16(origin, ref offset);
			return null;
		}
	}
}
