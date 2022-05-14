using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_79 : ITableObject
	{
		public uint unk_00;
		public uint unk_04;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.unk_00 = ReadUInt32(origin, ref offset);
			this.unk_04 = ReadUInt32(origin, ref offset);
			return null;
		}
	}
}
