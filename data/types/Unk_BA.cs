using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static DKSBE.MarshalHelper;

namespace DKSBE.data.types
{
	public unsafe class Unk_BA : ITableObject
	{
		public uint id;
		public string? equation;
		public uint unused;

		public Dictionary<long, PtrData?>? Read(byte* origin, ref long offset)
		{
			this.id = ReadUInt32(origin, ref offset);
			this.equation = ReadString(origin, ref offset);
			this.unused = ReadUInt32(origin, ref offset);
			return null;
		}
	}
}
